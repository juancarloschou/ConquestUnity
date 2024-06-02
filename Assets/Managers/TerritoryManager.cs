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
    private static TerritoryManager instance;

    private MapManager mapManager;
    private Territory territory;

    void Awake()
    {
        // Ensure only one instance exists.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This ensures the GameObject persists between scenes.
        }
        else
        {
            Destroy(gameObject);
        }

        // Additional initialization code...
    }

    public void Init(MapManager mapManager, Territory territory)
    {
        this.mapManager = mapManager;
        this.territory = territory;
    }

    void OnMouseDown()
    {
        mapManager.SelectTerritory(territory);
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

