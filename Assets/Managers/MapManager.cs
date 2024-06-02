using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets;

/// <summary>
/// -Generates the map
/// -Storages the list of territories
/// -Draw the map
/// Handle actions like SelectTerritory with TerritoryManager
/// </summary>
public class MapManager : MonoBehaviour
{
    private static MapManager instance;

    public Material territoryLineMaterial; // Material for the territory lines
    public Material selectedTerritoryMaterial; // Material for selected territory
    private List<Territory> territories; // Example
    private Territory selectedTerritory; // Currently selected territory

    private float thickLines = 0.25f; // Grosor de las líneas
    private string sortingLayerNameLines = "Foreground"; // Set the desired sorting layer
    private int sortingOrderLines = 1; // Set the desired sorting order

    //private string sortingLayerNameSprite = "Background"; // Set the desired sorting layer
    //private int sortingOrderSprite = 0; // Set the desired sorting order

    //public LineRenderer lineRendererMap;

    void Awake()
    {
        // Ensure only one instance exists.
        if (instance == null)
        {
            instance = this;
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
        /*
        // Set sorting layer and order in layer for the entire MapContainer (assuming it has a SpriteRenderer)
        SpriteRenderer mapSpriteRenderer = GetComponent<SpriteRenderer>();
        if (mapSpriteRenderer != null)
        {
            //mapSpriteRenderer.sortingLayerName = sortingLayerNameSprite;
            //mapSpriteRenderer.sortingOrder = sortingOrderSprite;
        }
        else
        {
            Debug.Log($"mapSpriteRenderer: {mapSpriteRenderer}");
        }
        */

        GenerateTerritories();

        //DrawTerritoryLines();
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
    


    // generate territories on the map
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

    
    // Draw territory lines using LineRenderer
    void DrawTerritoryLines()
    {
        Debug.Log("DrawTerritoryLines " + territories.Count);
        foreach (Territory territory in territories)
        {
            // Create a LineRenderer component on the MapContainer GameObject
            GameObject territoryObject = new GameObject(territory.territoryID + "Lines");
            territoryObject.transform.SetParent(transform);

            // Log the name of the parent
            Debug.Log($"{territoryObject.name} parent: {territoryObject.transform.parent.name}");

            // Add LineRenderer component to the GameObject
            LineRenderer lineRenderer = territoryObject.AddComponent<LineRenderer>();

            // Set LineRenderer properties
            lineRenderer.material = territoryLineMaterial; // Assign the line material
            lineRenderer.startWidth = thickLines;
            lineRenderer.endWidth = thickLines;
            lineRenderer.loop = true; // Connect the last point to the first one to form a closed loop

            // Set the sorting layer and order
            lineRenderer.sortingLayerName = sortingLayerNameLines;
            lineRenderer.sortingOrder = sortingOrderLines;

            // Set LineRenderer positions based on territoryBoundary points
            lineRenderer.positionCount = territory.territoryBoundary.Count;
            for (int i = 0; i < territory.territoryBoundary.Count; i++)
            {
                Vector2 mapPosition = territory.territoryBoundary[i];
                Vector3 worldPosition = new Vector3(mapPosition.x, mapPosition.y, 0);
                lineRenderer.SetPosition(i, worldPosition);
            }

            //PolygonCollider2D collider = territoryObject.AddComponent<PolygonCollider2D>();
            //collider.points = territory.territoryBoundary.ToArray();
            //territoryObject.AddComponent<TerritoryManager>().Init(this, territory);

            // Log the creation of the territory object
            Debug.Log($"Created territory object: {territoryObject.name}");
        }
    }
    

    public void SelectTerritory(Territory territory)
    {
        if (selectedTerritory != null)
        {
            // Reset previously selected territory color
            Transform previousObject = transform.Find(selectedTerritory.territoryID + "Lines");
            if (previousObject != null)
            {
                LineRenderer lineRenderer = previousObject.GetComponent<LineRenderer>();
                lineRenderer.material = territoryLineMaterial;
            }
        }

        // Select new territory
        selectedTerritory = territory;
        Transform selectedObject = transform.Find(territory.territoryID + "Lines");
        if (selectedObject != null)
        {
            LineRenderer lineRenderer = selectedObject.GetComponent<LineRenderer>();
            lineRenderer.material = selectedTerritoryMaterial;
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



/*
        territories.Add(new Territory("SampleTerritory" + (number++).ToString(), 10, 5, GenerateVectorsTerritory(new Vector2(15, 15), new Vector2(6, 6))));
        territories.Add(new Territory("SampleTerritory" + (number++).ToString(), 10, 5, GenerateVectorsTerritory(new Vector2(9, 9), new Vector2(6, 6))));
        territories.Add(new Territory("SampleTerritory" + (number++).ToString(), 10, 5, GenerateVectorsTerritory(new Vector2(3, 3), new Vector2(6, 6))));
        territories.Add(new Territory("SampleTerritory" + (number++).ToString(), 10, 5, GenerateVectorsTerritory(new Vector2(-3, -3), new Vector2(6, 6))));
        territories.Add(new Territory("SampleTerritory" + (number++).ToString(), 10, 5, GenerateVectorsTerritory(new Vector2(-9, -9), new Vector2(6, 6))));
        territories.Add(new Territory("SampleTerritory" + (number++).ToString(), 10, 5, GenerateVectorsTerritory(new Vector2(-15, -15), new Vector2(6, 6))));
        territories.Add(new Territory("SampleTerritory" + (number++).ToString(), 10, 5, GenerateVectorsTerritory(new Vector2(-21, -21), new Vector2(6, 6))));
*/

/*
       // test draw lines: lineRenderer
       if (lineRendererMap == null)
       {
           Debug.LogError("lineRendererMap not assigned.");
       }
       else
       {
           lineRendererMap.material = territoryLineMaterial; // Assign the line material
           lineRendererMap.startWidth = thickLines;
           lineRendererMap.endWidth = thickLines;

           // Set the sorting layer and order
           lineRendererMap.sortingLayerName = sortingLayerNameLines;
           lineRendererMap.sortingOrder = sortingOrderLines;

           // Set positions for the trail points
           Vector3[] positions = new Vector3[]
           {
               new Vector3(0, 0, 0),
               new Vector3(1, 1, 0),
               new Vector3(2, 0, 0),
               new Vector3(3, 1, 0),
           };

           // Set LineRenderer positions based on positions points
           lineRendererMap.positionCount = positions.Length;
           lineRendererMap.SetPositions(positions);
       }
       */


/*
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

public class MapContainer : MonoBehaviour
{
    public Material territoryLineMaterial; // Material for the territory lines
    private List<GameObject> territoryObjects; // List to store territory GameObjects
    private float thickLines = 0.5f; // grosor de la slineas
    public string sortingLayerNameLines = "ForeGround"; // Set the desired sorting layer
    public int sortingOrderLines = 1; // Set the desired sorting order
    public string sortingLayerNameSprite = "BackGround"; // Set the desired sorting layer
    public int sortingOrderSprite = 0; // Set the desired sorting order

    // Start is called before the first frame update
    void Start()
    {
        // Set sorting layer and order in layer for the entire MapContainer (assuming it has a SpriteRenderer)
        SpriteRenderer mapSpriteRenderer = GetComponent<SpriteRenderer>();
        if (mapSpriteRenderer != null)
        {
            mapSpriteRenderer.sortingLayerName = sortingLayerNameSprite;
            mapSpriteRenderer.sortingOrder = sortingOrderSprite;
        }
        else
        {
            Debug.Log($"mapSpriteRenderer: {mapSpriteRenderer}");
        }

        // Example: Generate and draw a sample territory
        List<Vector2> territoryBoundary = new List<Vector2>
        {
            new Vector2(2, 2),
            new Vector2(2, 8),
            new Vector2(8, 8),
            new Vector2(8, 2),
        };

        territoryObjects = new List<GameObject>();
        territoryObjects.Add(CreateTerritory("SampleTerritory", territoryBoundary));

        territoryBoundary = new List<Vector2>
        {
            new Vector2(-2, -2),
            new Vector2(-2, -8),
            new Vector2(-8, -8),
            new Vector2(-8, -2),
        };
        territoryObjects.Add(CreateTerritory("SampleTerritory2", territoryBoundary));

        DrawTerritoryLines();
    }

    // Draw territory lines using LineRenderer
    void DrawTerritoryLines()
    {
        foreach (GameObject territoryObject in territoryObjects)
        {
            LineRenderer lineRenderer = territoryObject.GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                Debug.LogError("LineRenderer not found on territory object.");
                continue;
            }

            // Set LineRenderer properties
            lineRenderer.material = territoryLineMaterial; // Assign the line material
            lineRenderer.startWidth = thickLines;
            lineRenderer.endWidth = thickLines;

            // Connect the last point to the first one to form a closed loop
            lineRenderer.loop = true;

            // Set the sorting layer and order
            lineRenderer.sortingLayerName = sortingLayerNameLines;
            lineRenderer.sortingOrder = sortingOrderLines;
        }
    }

    // Create a territory represented by an empty GameObject with a BoxCollider2D
    GameObject CreateTerritory(string territoryID, List<Vector2> territoryBoundary)
    {
        GameObject territoryObject = new GameObject(territoryID);
        territoryObject.transform.SetParent(transform);

        // Add BoxCollider2D to the GameObject
        BoxCollider2D collider = territoryObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Adjust as needed

        // Add LineRenderer component to the GameObject
        LineRenderer lineRenderer = territoryObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = territoryBoundary.Count;

        // Set LineRenderer positions based on territoryBoundary points
        for (int i = 0; i < territoryBoundary.Count; i++)
        {
            Vector2 mapPosition = territoryBoundary[i];
            Vector3 worldPosition = new Vector3(mapPosition.x, mapPosition.y, 0);

            lineRenderer.SetPosition(i, worldPosition);
        }

        return territoryObject;
    }
}
*/



/*
using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContainer : MonoBehaviour
{
    public Vector2 mapDimensions = new Vector2(10, 10); // Default dimensions
    public Material territoryLineMaterial; // Material for the territory lines
    private List<Territory> sampleTerritories; // Example

    // Start is called before the first frame update
    void Start()
    {
        // Example: Generate and draw a sample territory
        List<Vector2> territoryBoundary = new List<Vector2>
        {
            new Vector2(2, 2),
            new Vector2(2, 8),
            new Vector2(8, 8),
            new Vector2(8, 2),
        };

        sampleTerritories = new List<Territory>();
        sampleTerritories.Add(new Territory("SampleTerritory", 10, 5, territoryBoundary));

        territoryBoundary = new List<Vector2>
        {
            new Vector2(-2, -2),
            new Vector2(-2, -8),
            new Vector2(-8, -8),
            new Vector2(-8, -2),
        };
        sampleTerritories.Add(new Territory("SampleTerritory2", 10, 5, territoryBoundary));

        DrawTerritoryLines(sampleTerritories);
    }

    // Draw territory lines using LineRenderer
    void DrawTerritoryLines(List<Territory> territories)
    {
        foreach (Territory territory in territories)
        {
            // Create a new empty GameObject for the territory lines
            GameObject territoryObject = new GameObject(territory.territoryID + "Lines");
            territoryObject.transform.SetParent(transform);

            // Add LineRenderer component to the GameObject
            LineRenderer lineRenderer = territoryObject.AddComponent<LineRenderer>();
            lineRenderer.material = territoryLineMaterial; // Assign the line material

            // Set LineRenderer properties
            lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 0.1f); // Adjust width as needed
            lineRenderer.positionCount = territory.territoryBoundary.Count;

            // Set LineRenderer positions based on territoryBoundary points
            for (int i = 0; i < territory.territoryBoundary.Count; i++)
            {
                Vector2 mapPosition = territory.territoryBoundary[i];
                Vector3 worldPosition = new Vector3(mapPosition.x, mapPosition.y, 0); // Assuming 2D map

                lineRenderer.SetPosition(i, worldPosition);
            }

            // Set sorting layer and order in layer for LineRenderer
            lineRenderer.sortingLayerName = "Foreground";
            lineRenderer.sortingOrder = 1;
        }

        // Set sorting layer and order in layer for the entire MapContainer (assuming it has a SpriteRenderer)
        SpriteRenderer mapSpriteRenderer = GetComponent<SpriteRenderer>();
        if (mapSpriteRenderer != null)
        {
            mapSpriteRenderer.sortingLayerName = "Background";
            mapSpriteRenderer.sortingOrder = 0;
        }
    }
}
*/