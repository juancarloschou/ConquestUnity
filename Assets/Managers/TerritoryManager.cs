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

    public void SelectTerritory(TerritoryController territory)
    {
        // Deselect the previous territory if any
        if (selectedTerritory != null)
        {
            selectedTerritory.Deselect();
        }

        // Select the new territory
        selectedTerritory = territory;
        selectedTerritory.Select();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

