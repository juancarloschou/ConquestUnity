using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets;

/// <summary>
/// - Generates the map
/// - Storages the list of territories of the map (central information of territories)
/// - Draw the map (generates the prefabs)
/// Handle actions like SelectTerritory with TerritoryManager
/// </summary>
public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    // Asignar el Prefab Territory
    public GameObject territoryPrefab;

    // Storages the list of territories of the map (IMPORTANT central information of territories)
    public List<Territory> territories; 

    // For drawing the territories lines of the map, and draw selected territory
    public Material territoryLineMaterial; // Material for the territory lines
    public Material selectedTerritoryMaterial; // Material for selected territory

    private float lineWidth = 0.25f; // Grosor de las líneas
    //private string sortingLayerNameLines = "Foreground"; // Set the desired sorting layer
    //private int sortingOrderLines = 1; // Set the desired sorting order

    // For drawing the staring basement of the players


    void Awake()
    {
        // Ensure only one instance exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This ensures the GameObject persists between scenes.

            SceneManager.sceneLoaded += OnSceneLoaded;
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
        GenerateTerritories();
    }

    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // To ensure that DrawTerritoryLines() is only executed in the gameScene and not in the mainScene,
        // you can use Unity's SceneManager to detect when the active scene changes and then call the method accordingly.
        if (scene.name == "GameScene")
        {
            Debug.Log("MapManeger OnSceneLoaded GameScene");
            DrawTerritoryLines();

            /*
            // Optional: Log parents of all territory objects
            foreach (Transform child in transform)
            {
                LogParent(child.gameObject);
            }
            */
        }
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Generate territories on the map
    void GenerateTerritories()
    {
        // Example: Generate and draw a sample territory
        territories = new List<Territory>();

        Vector2 totalMapInitial = new Vector2(-16, -16);
        Vector2 totalMapSize = new Vector2(32, 32);
        Vector2 territorySize = new Vector2(4, 4);
        int number = 1;

        // Calculate the number of territories that fit in each dimension
        int horizontalCount = (int)(totalMapSize.x / territorySize.x);
        int verticalCount = (int)(totalMapSize.y / territorySize.y);

        // Nested loops to generate territories
        for (int i = 0; i < horizontalCount; i++)
        {
            for (int j = 0; j < verticalCount; j++)
            {
                Vector2 initialPoint = new Vector2(
                    totalMapInitial.x + i * territorySize.x,
                    totalMapInitial.y + j * territorySize.y
                );

                List<Vector2> territoryBoundary = GenerateVectorsTerritory(initialPoint, territorySize);
                territories.Add(new Territory("SampleTerritory" + (number++).ToString(), 10, 5, territoryBoundary));
            }
        }
    }

    List<Vector2> GenerateVectorsTerritory(Vector2 initialPoint, Vector2 size)
    {
        return new List<Vector2>
        {
            initialPoint,
            new Vector2(initialPoint.x, initialPoint.y + size.y), // Top-left corner
            new Vector2(initialPoint.x + size.x, initialPoint.y + size.y), // Top-right corner
            new Vector2(initialPoint.x + size.x, initialPoint.y) // Bottom-right corner
        };
    }

    
    // Draw territory lines using LineRenderer, based on territories list
    // Generate the game objects as childs of MapManager
    void DrawTerritoryLines()
    {
        Debug.Log("DrawTerritoryLines " + territories.Count);
        foreach (Territory territory in territories)
        {
            // Instantiate the territory prefab and set MapContainer as its parent
            GameObject territoryObject = Instantiate(territoryPrefab, transform);

            // Log the name of the object and the parent
            Debug.Log($"{territoryObject.name} parent: {territoryObject.transform.parent.name}");

            /// Initialize the TerritoryController script
            TerritoryController territoryController = territoryObject.GetComponent<TerritoryController>();
            territoryController.Init(territory, territoryLineMaterial, lineWidth);

            // Log the creation of the territory object
            Debug.Log($"Created territory object: {territoryObject.name}");
        }
    }

    /*
    void LogParent(GameObject obj)
    {
        if (obj.transform.parent != null)
        {
            Debug.Log($"{obj.name} parent: {obj.transform.parent.name}");
        }
        else
        {
            Debug.Log($"{obj.name} has no parent");
        }
    }
    */
}

