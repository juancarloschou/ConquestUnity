using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// - Controls tower-related functionalities, including construction and defense calculations.
/// Construction Logic of towers:
/// Manages tower construction in specified territories.
/// Collaborates with the UIManager for visual feedback.
/// - Construction of barraks and other buildings
/// </summary>
public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager Instance { get; private set; }

    // For drawing the staring basement of the players
    public Sprite basementSprite; // Asign the sprite of the town

    private float scaleBasement = 0.9f;

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
        // Check that references are asigned
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
            Debug.Log("BuildingsManager OnSceneLoaded GameScene");
            CreateBasements(); // draw starting basement (spawn basement)
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void CreateBasements()
    {
        Debug.Log("CreateBasements");
        foreach (Territory territory in MapManager.Instance.territories)
        {
            if (territory.BasementPlayer > 0)
            {
                Vector2 center = MapManager.Instance.GetTerritoryCenter(territory.TerritoryBoundary);

                GameObject basementObject = new GameObject("Basement" + territory.BasementPlayer);
                basementObject.transform.SetParent(transform);
                basementObject.transform.position = center;

                // Add SpriteRenderer component
                SpriteRenderer spriteRenderer = basementObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = basementSprite;

                // Set the sorting layer and order for the basement
                spriteRenderer.sortingLayerName = MapManager.Instance.sortingLayerNameMap;
                spriteRenderer.sortingOrder = MapManager.Instance.sortingOrderBuildings;

                // Adjust scale if necessary
                basementObject.transform.localScale = MapManager.Instance.CalculateScaleFromTerritory(territory.TerritoryBoundary, 
                    spriteRenderer, scaleBasement);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}