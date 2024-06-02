using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Controls UI elements, such as resource displays and territory information.
/// Updates troop quantities, resource indicators, and other UI elements.
/// Receives information from relevant managers (e.g., TroopManager, ResourceManager).
/// - Manages transitions between different screens.
/// Controls the activation and deactivation of UI elements based on the active screen.
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager instance;

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
