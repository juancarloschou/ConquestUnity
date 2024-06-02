using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Handles creation of the map, territories, connections
/// Assign of special territories (basements, high resources, tower)
/// - Map editor
/// </summary>
public class MapGeneratorManager : MonoBehaviour
{
    public static MapGeneratorManager Instance { get; private set; }

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