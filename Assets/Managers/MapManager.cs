using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets;
using Unity.VisualScripting;

/// <summary>
/// - Generates the map
/// - Storages the list of territories of the map (central information of territories)
/// - Draw the map (generates the prefabs of territories)
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

    public Color neutralTerritoryColor = Color.grey; // color of neutral territory

    // For drawing the staring basement of the players
    public Sprite basementSprite; // Asign the sprite of the town 

    // layer and order of lines (boundaries)
    private string sortingLayerNameLines = "Default"; // Set the desired sorting layer
    private int sortingOrderLines = 2; // Set the desired sorting order 

    // layer and order of filling (territory)
    private string sortingLayerNameFilling = "Default"; // Set the desired sorting layer
    private int sortingOrderFilling = 1; // Set the desired sorting order 

    // layer and order of building (basement)
    private string sortingLayerNameBuilings = "Default"; // Set the desired sorting layer
    private int sortingOrderBuildings = 5; // Set the desired sorting order 


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
        // Comprobamos que las referencias estén asignadas
        if (territoryPrefab == null)
        {
            Debug.LogError("territoryPrefab is not assigned in the Inspector.");
            return;
        }
        if (basementSprite == null)
        {
            Debug.LogError("basementSprite is not assigned in the Inspector.");
            return;
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // To ensure that DrawTerritoryLines() is only executed in the gameScene and not in the mainScene,
        // you can use Unity's SceneManager to detect when the active scene changes and then call the method accordingly.
        if (scene.name == "GameScene")
        {
            Debug.Log("MapManager OnSceneLoaded GameScene");
            GenerateTerritories();
            DrawTerritoryLines();
            DrawInitialBasements();
        }
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Generate territories on the map, set the initial basements and territories of each player
    void GenerateTerritories()
    {
        int selectedPlayerQuantity = GameManager.Instance.GetSelectedPlayerQuantity();
        Debug.Log($"GenerateTerritories selectedPlayerQuantity {selectedPlayerQuantity}");

        territories = new List<Territory>();

        Vector2 totalMapInitial = new Vector2(-16, -16);
        Vector2 totalMapSize = new Vector2(32, 32);
        Vector2 territorySize = new Vector2(8, 8);
        int number = 1;

        int horizontalCount = (int)(totalMapSize.x / territorySize.x);
        int verticalCount = (int)(totalMapSize.y / territorySize.y);
        int totalTerritories = horizontalCount * verticalCount;

        // Define the corners for each player based on the number of players
        Vector2[] corners = GenerateCorners(selectedPlayerQuantity, totalMapInitial, totalMapSize, territorySize);

        for (int i = 0; i < horizontalCount; i++)
        {
            for (int j = 0; j < verticalCount; j++)
            {
                Vector2 initialPoint = new Vector2(
                    totalMapInitial.x + i * territorySize.x,
                    totalMapInitial.y + j * territorySize.y
                );

                List<Vector2> territoryBoundary = GenerateVectorsTerritory(initialPoint, territorySize);

                // Assign player and basementPlayer based on corner position
                int player = 0; // neutral
                int basementPlayer = 0;

                for (int k = 0; k < corners.Length; k++)
                {
                    if (initialPoint == corners[k])
                    {
                        player = k + 1;
                        basementPlayer = k + 1;
                        //Debug.Log($"player {player}, basementPlayer {basementPlayer}, corners.Length {corners.Length}");
                        break;
                    }
                }

                territories.Add(new Territory("Territory" + number, 0, 0, player, basementPlayer, territoryBoundary));
                number++;
            }
        }
    }

    // Define the corners for each player based on the number of players
    Vector2[] GenerateCorners(int selectedPlayerQuantity, Vector2 totalMapInitial, Vector2 totalMapSize, Vector2 territorySize)
    {
        Vector2[] corners = new Vector2[selectedPlayerQuantity];
        switch (selectedPlayerQuantity)
        {
            case 2: // Two players, opposite corners
                corners[0] = new Vector2(totalMapInitial.x, totalMapInitial.y); // Bottom left
                corners[1] = new Vector2(totalMapInitial.x + totalMapSize.x - territorySize.x, totalMapInitial.y + totalMapSize.y - territorySize.y); // Top right
                break;
            case 3: // Three players, one player in each corner and the remaining one randomly placed
                corners[0] = new Vector2(totalMapInitial.x, totalMapInitial.y); // Bottom left
                corners[1] = new Vector2(totalMapInitial.x + totalMapSize.x - territorySize.x, totalMapInitial.y + totalMapSize.y - territorySize.y); // Top right

                // Randomly choose one of the two remaining corners for the third player
                if (Random.value < 0.5f)
                {
                    corners[2] = new Vector2(totalMapInitial.x, totalMapInitial.y + totalMapSize.y - territorySize.y); // Top left
                }
                else
                {
                    corners[2] = new Vector2(totalMapInitial.x + totalMapSize.x - territorySize.x, totalMapInitial.y); // Bottom right
                }
                break;
            case 4: // Four players, each player in one corner
                corners[0] = new Vector2(totalMapInitial.x, totalMapInitial.y); // Bottom left
                corners[1] = new Vector2(totalMapInitial.x + totalMapSize.x - territorySize.x, totalMapInitial.y); // Bottom right
                corners[2] = new Vector2(totalMapInitial.x, totalMapInitial.y + totalMapSize.y - territorySize.y); // Top left
                corners[3] = new Vector2(totalMapInitial.x + totalMapSize.x - territorySize.x, totalMapInitial.y + totalMapSize.y - territorySize.y); // Top right
                break;
            default:
                Debug.LogError("Invalid number of players!");
                break;
        }
        return corners;
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
            //Debug.Log($"{territoryObject.name} parent: {territoryObject.transform.parent.name}");

            /// Initialize the TerritoryController script
            TerritoryController territoryController = territoryObject.GetComponent<TerritoryController>();

            // Get the color of the player for this territory
            Color territoryColor = neutralTerritoryColor;
            if (territory.Player != 0)
            {
                territoryColor = GameManager.Instance.GetPlayerColor(territory.Player);
                Debug.Log($"territory.Id {territory.Id}, territory.Player {territory.Player}, territoryColor {territoryColor}, territory.BasementPlayer {territory.BasementPlayer}");
            }

            // Initialize the TerritoryController with territory data and player color
            territoryController.Init(territory, territoryLineMaterial, lineWidth, territoryColor, sortingLayerNameLines, sortingOrderLines, sortingLayerNameFilling, sortingOrderFilling);

            // Log the creation of the territory object
            //Debug.Log($"Created territory object: {territoryObject.name}");
        }
    }

    void DrawInitialBasements()
    {
        Debug.Log("DrawInitialBasements");
        foreach (Territory territory in territories)
        {
            if (territory.BasementPlayer > 0)
            {
                Vector2 center = GetTerritoryCenter(territory.TerritoryBoundary);

                GameObject basementObject = new GameObject("Basement" + territory.BasementPlayer);
                basementObject.transform.SetParent(transform);
                basementObject.transform.position = center;

                // Add SpriteRenderer component
                SpriteRenderer spriteRenderer = basementObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = basementSprite;

                // Set the sorting layer and order for the basement
                spriteRenderer.sortingLayerName = sortingLayerNameBuilings;
                spriteRenderer.sortingOrder = sortingOrderBuildings;

                // Adjust scale if necessary
                float territoryArea = CalculateTerritoryArea(territory.TerritoryBoundary);
                //Debug.Log("territoryArea " + territoryArea);
                float spriteArea = basementSprite.bounds.size.x * basementSprite.bounds.size.y;
                //Debug.Log("spriteArea " + spriteArea);
                float scale = Mathf.Sqrt(territoryArea / spriteArea); // Calculate scale based on territory area and sprite area
                scale = scale * (float)0.75;
                scale = Mathf.RoundToInt(scale); // Round the scale to the nearest integer
                //Debug.Log("scale " + scale);
                basementObject.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }

    public Vector2 GetTerritoryCenter(List<Vector2> boundary)
    {
        float x = 0;
        float y = 0;
        foreach (Vector2 point in boundary)
        {
            x += point.x;
            y += point.y;
        }
        return new Vector2(x / boundary.Count, y / boundary.Count);
    }

    // Function to calculate the area of the territory
    public float CalculateTerritoryArea(List<Vector2> boundary)
    {
        float area = 0f;
        int numPoints = boundary.Count;
        for (int i = 0; i < numPoints; i++)
        {
            Vector2 current = boundary[i];
            Vector2 next = boundary[(i + 1) % numPoints];
            area += (next.x - current.x) * (next.y + current.y);
        }
        area *= 0.5f; // Take half of the sum
        return Mathf.Abs(area); // Return the absolute value of the area
    }
}

