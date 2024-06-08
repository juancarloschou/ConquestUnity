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

    public void SelectTerritory(TerritoryController territoryController)
    {
        // Deselect the previous territory if any
        if (selectedTerritory != null)
        {
            selectedTerritory.Deselect();
        }

        // Select the new territory
        selectedTerritory = territoryController;
        selectedTerritory.Select();

        // check if basement selected to show buttons actions
        bool basement = false;
        Territory territory = MapManager.Instance.GetTerritoryById(selectedTerritory.territoryId);
        if(territory != null)
        {
            if(territory.BasementPlayer == GameManager.Instance.currentPlayer)
            {
                basement = true;
            }
        }

        if(basement)
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

    public bool IsTerritorySelected()
    {
        return selectedTerritory != null;
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

    // Update is called once per frame
    void Update()
    {

    }
}

