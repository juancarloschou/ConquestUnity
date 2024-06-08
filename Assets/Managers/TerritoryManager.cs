using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// - Handles territory-related functionalities.
/// Controls territory selection, troop movement, and resource generation, related to the map level.
/// - Selection Logic:
/// Handles territory selection through tapping.
/// Communicates with the UIManager for updating selected territories.
/// </summary>
public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager Instance { get; private set; }

    private TerritoryController selectedTerritory; // Currently selected territory

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
        selectedTerritory = null;
    }

    public bool IsTerritorySelected()
    {
        return selectedTerritory != null;
    }

    public void SelectTerritory(TerritoryController territoryController)
    {
        Territory territory = MapManager.Instance.GetTerritoryById(territoryController.territoryId);
        if (territory != null)
        {
            // if troop selected, its a possible movement
            if (TroopManager.Instance.IsTroopSelected())
            {
                // its a movement, not a selection
                // check destination (not same as origin)
                int territoryIdOrigin = (int)TroopManager.Instance.TroopSelectedTerritoryId();
                int territoryIdDestination = territoryController.territoryId;
                if (TroopManager.Instance.CheckMovement(territoryIdOrigin, territoryIdDestination))
                {
                    TroopManager.Instance.CreateMovement(GameManager.Instance.currentPlayer, territoryIdOrigin, territoryIdDestination,
                        GetDefendersStrengthFromTerritoryId(territoryIdOrigin));
                }
                else
                {
                    Debug.Log("cancel movement, no CheckMovement");
                }
            }
            else
            {
                // no troop selected, so its a selection, not a movement
                // select territory, but if there is a troop on territory, select the troop

                // Deselect the previous territory if any
                if (selectedTerritory != null)
                {
                    selectedTerritory.Deselect();
                }

                TroopController troopController = territory.TroopController;
                if (troopController != null)
                {
                    // there is troop on territory, select troop
                    TroopManager.Instance.SelectTroop(troopController);
                }
                else
                {
                    // if there is no troop, select the new territory
                    selectedTerritory = territoryController;
                    selectedTerritory.Select();

                    //TroopManager.Instance.DeselectTroop(); // cant happen that troop was selected and now select territory (no move)
                }

                // check if basement selected to show buttons actions (either if territory or troop selected)
                if (territory.BasementPlayer == GameManager.Instance.currentPlayer)
                {
                    // show button create kinght
                    UIManager.Instance.ShowButtonCreateKnight();
                }
                else
                {
                    // hide button create kinght
                    UIManager.Instance.HideButtonCreateKnight();
                }
            }
        }
    }

    public bool SelectedTerritoryHasBasementCurrentPlayer()
    {
        if (selectedTerritory != null)
        {
            Territory territory = MapManager.Instance.GetTerritoryById(selectedTerritory.territoryId);
            if (territory != null)
            {
                if (territory.BasementPlayer == GameManager.Instance.currentPlayer)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public TerritoryController GetSelectedTerritory()
    {
        return selectedTerritory;
    }

    public int GetDefendersStrengthFromTerritoryId(int id)
    {
        Territory territory = MapManager.Instance.GetTerritoryById(id);
        if(territory != null)
        {
            return territory.DefendersStrength;
        }
        else 
        { 
            return 0; 
        }
    }

    public TerritoryController GetTerritoryById(int id)
    {
        Territory territory = MapManager.Instance.GetTerritoryById(id);
        if(territory != null )
        {
            return territory.territoryController;
        }
        else
        {
            return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

