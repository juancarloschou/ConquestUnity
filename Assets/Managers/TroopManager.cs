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

    private float scaleKnight = 0.75f;

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

    public void CreateTroopKnight()
    {
        if (TerritoryManager.Instance.IsTerritorySelected() && TerritoryManager.Instance.SelectedTerritoryHasBasementCurrentPlayer())
        {
            TerritoryController selectedTerritory = TerritoryManager.Instance.GetSelectedTerritory();

            // Check if the player has enough resources
            if (ResourceManager.Instance.HasEnoughResources(knightCost))
            {
                Debug.Log("created knight");

                // spawn troop
                Territory territory = MapManager.Instance.GetTerritoryById(selectedTerritory.territoryId);
                if(territory != null)
                {
                    // if troop already exists increase it, if not spawn
                    if (territory.DefendersStrength == 0)
                    {
                        // Instantiate the troop at the center of the territory
                        Vector3 spawnPosition = MapManager.Instance.GetTerritoryCenter(territory.TerritoryBoundary);

                        GameObject knightObject = Instantiate(knightPrefab, spawnPosition, Quaternion.identity, this.transform);

                        // Log the name of the object and the parent
                        Debug.Log($"{knightObject.name} parent: {knightObject.transform.parent.name}");

                        SpriteRenderer knightSpriteRenderer = knightObject.GetComponent<SpriteRenderer>();
                        if (knightSpriteRenderer != null)
                        {
                            // Set the sorting layer and order for the basement
                            knightSpriteRenderer.sortingLayerName = MapManager.Instance.sortingLayerNameMap;
                            knightSpriteRenderer.sortingOrder = MapManager.Instance.sortingOrderTroops;

                            knightSpriteRenderer.transform.localScale = MapManager.Instance.CalculateScaleFromTerritory(
                                territory.TerritoryBoundary, knightSpriteRenderer, scaleKnight);
                        }
                    }

                    // Update troop quantity in the territory (you can implement this as needed)
                    territory.DefendersStrength += 1;

                    // Deduct resources
                    ResourceManager.Instance.DeductResources(knightCost);
                }
                else
                {
                    Debug.LogError("Error in CreateTroop GetTerritoryById, " + selectedTerritory.territoryId);
                }
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

    // Update is called once per frame
    void Update()
    {

    }
}
