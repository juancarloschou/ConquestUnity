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
    private static TroopManager instance;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
