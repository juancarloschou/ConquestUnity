using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Manages troop creation, spawning, and movements.
/// Troop Movement:
/// Manages troop movement logic based on player input.
/// Coordinates with the TerritoryManager for destination information.
/// - Handles battles and updates troop quantities.
/// Initiates real-time battles.
/// Communicates results to the UIManager for display.
/// </summary>
public class TroopManager : MonoBehaviour
{
    public static TroopManager Instance { get; private set; }

    // Assign the Prefab troop
    public GameObject knightPrefab;

    public int knightCost = 5; // cost of the knight
    private bool isSpawningTroop;
    private float timeCreateKnigth = 3f; // 5 sg

    private float scaleKnight = 0.6f;

    private TroopController selectedTroop; // Currently selected troop

    public List<Movement> movements; // movements of troops

    void Awake()
    {
        // Ensure only one instance exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This ensures the GameObject persists between scenes.
        }
        else
        {
            Destroy(gameObject);
        }

        // Additional initialization code...
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitializeGame()
    {
        selectedTroop = null;
        isSpawningTroop = false;
    }

    public bool IsTroopSelected()
    { 
        return selectedTroop != null;
    }

    public int? TroopSelectedTerritoryId()
    {
        return selectedTroop.territoryId;
    }

    public void CreateTroopKnight()
    {
        if (isSpawningTroop)
        {
            Debug.Log("A troop is already being spawned. Please wait.");
            return;
        }

        if (TerritoryManager.Instance.IsTerritorySelected() && TerritoryManager.Instance.SelectedTerritoryHasBasementCurrentPlayer())
        {
            TerritoryController selectedTerritory = TerritoryManager.Instance.GetSelectedTerritory();

            if (ResourceManager.Instance.HasEnoughResources(knightCost))
            {
                Debug.Log("Started creating knight...");
                StartCoroutine(SpawnKnightWithDelay(selectedTerritory, timeCreateKnigth));

                // mark the building with working image
                //...
            }
            else
            {
                Debug.Log("Not enough resources to create a troop.");
            }
        }
        else
        {
            Debug.Log("No territory selected or selected territory does not have a basement.");
        }
    }

    private IEnumerator SpawnKnightWithDelay(TerritoryController selectedTerritory, float delay)
    {
        isSpawningTroop = true;
        yield return new WaitForSeconds(delay);

        // spawn troop
        Territory territory = MapManager.Instance.GetTerritoryById(selectedTerritory.territoryId);
        if (territory != null)
        {
            TroopController troopController;
            if (territory.DefendersStrength == 0)
            {
                Vector3 spawnPosition = MapManager.Instance.GetTerritoryCenter(territory.TerritoryBoundary);
                GameObject knightObject = Instantiate(knightPrefab, spawnPosition, Quaternion.identity, this.transform);

                troopController = knightObject.GetComponent<TroopController>();
                if (troopController != null)
                {
                    troopController.Init(territory.Id);
                    territory.TroopController = troopController;
                }

                SpriteRenderer knightSpriteRenderer = knightObject.GetComponent<SpriteRenderer>();
                if (knightSpriteRenderer != null)
                {
                    knightSpriteRenderer.sortingLayerName = MapManager.Instance.sortingLayerNameMap;
                    knightSpriteRenderer.sortingOrder = MapManager.Instance.sortingOrderTroops;
                    knightSpriteRenderer.transform.localScale = MapManager.Instance.CalculateScaleFromTerritory(
                        territory.TerritoryBoundary, knightSpriteRenderer, scaleKnight);
                }
            }
            else
            {
                troopController = territory.TroopController;
            }

            territory.DefendersStrength += 1;

            if (troopController != null)
            {
                troopController.SetDefendersStrength(territory.DefendersStrength);
            }

            ResourceManager.Instance.DeductResources(knightCost);
        }
        else
        {
            Debug.LogError("Error in CreateTroop GetTerritoryById, " + selectedTerritory.territoryId);
        }

        isSpawningTroop = false;
        Debug.Log("Knight created successfully.");
    }

    public void DeselectTroop()
    {
        // Deselect the previous troop if any
        if (selectedTroop != null)
        {
            selectedTroop.Deselect();
        }
    }

    public void CancelSelection()
    {
        DeselectTroop();

        // Hide movement options and hide cancel selection
        UIManager.Instance.HideMovementOptions();
    }

    public void SelectTroop(TroopController troopController)
    {
        // Deselect the previous troop if any
        if (selectedTroop != null)
        {
            selectedTroop.Deselect();
        }

        // Select the new troop
        selectedTroop = troopController;
        selectedTroop.Select();

        // Show movement options and show cancel selection
        UIManager.Instance.ShowMovementOptions(troopController.defendersStrength);
    }

    public bool CheckMovement(int originTerritoryId, int destinationTerritoryId)
    {
        if (originTerritoryId != destinationTerritoryId) 
        { 
            return true; 
        }
        else
        {
            return false;
        }
    }

    public void CreateMovement(int player, int territoryIdOrigin, int territoryIdDestination, int defendersStrength)
    {
        // Called when user taps on destination territory

        if (selectedTroop == null)
        {
            Debug.Log("Error in CreateMovement, selectedTroop null");
            return;
        }

        TerritoryController destinationTerritoryController = TerritoryManager.Instance.GetTerritoryById(territoryIdDestination);
        if (destinationTerritoryController == null)
        {
            Debug.Log("Error in CreateMovement, destinationTerritoryController null"); 
            return;
        }

        // the territory origin will not have troops anymore
        Territory originTerritory = MapManager.Instance.GetTerritoryById(territoryIdOrigin);
        if (originTerritory == null)
        {
            Debug.Log("Error in CreateMovement, originTerritory null");
            return;
        }
        originTerritory.DefendersStrength = 0;
        originTerritory.TroopController = null;

        // Show movement arrow (should be here with funcion DrawMovements)
        //UIManager.Instance.ShowMovementArrow(selectedTroop, destinationTerritory);
        // !!!

        // Calculate movement duration based on distance
        float distance = Vector3.Distance(selectedTroop.transform.position, destinationTerritoryController.transform.position);
        float duration = distance / 5.0f; // Example speed

        // Start moving the troop
        StartCoroutine(selectedTroop.MoveTo(destinationTerritoryController, duration));

        // Hide movement options and hide cancel selection
        UIManager.Instance.HideMovementOptions();
    }

    // if the user cancel manually the movement (also can be cancelled automatically)
    public void CancelTroopMovement()
    {
        if (selectedTroop == null)
        {
            Debug.Log("Error CancelTroopMovement, selectedTroop null");
            return;
        }

        // Stop movement coroutine and send troop to nearest territory
        StopCoroutine(selectedTroop.MoveTo(null, 0));
        //selectedTroop.transform.position = selectedTroop.GetNearestTerritory().transform.position;

        // Reset state
        //selectedTroop.Deselect();
        //selectedTroop = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
