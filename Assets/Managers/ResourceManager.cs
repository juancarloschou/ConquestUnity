using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Manages resource generation based on owned territories.
/// Communicates with the UIManager for UI updates.
/// - Handles special resource territories and bonuses.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

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

    // Update is called once per frame
    void Update()
    {

    }
}