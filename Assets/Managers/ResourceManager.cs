using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// - Manages resource generation based on owned territories.
/// Communicates with the UIManager for UI updates.
/// - Handles special resource territories and bonuses.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    // For drawing the resources quantity
    public Sprite resourceSprite; // Asign the sprite of the resource

    private float resources;
    private float resourceGenerationInterval = 1f; // Interval in seconds for resource generation

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Check that references are asigned
        if (resourceSprite == null)
        {
            Debug.LogError("resourceSprite is not assigned in the Inspector.");
            return;
        }
    }

    public void InitializeGame()
    {
        resources = 0;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // To ensure that DrawTerritoryLines() is only executed in the gameScene and not in the mainScene,
        // you can use Unity's SceneManager to detect when the active scene changes and then call the method accordingly.
        if (scene.name == "GameScene")
        {
            Debug.Log("ResourceManager OnSceneLoaded GameScene");

            UIManager.Instance.ShowResources(resources, resourceSprite); // Initialize resource display

            StartCoroutine(GenerateResources()); // start routine
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator GenerateResources()
    {
        while (GameManager.Instance.alive)
        {
            yield return new WaitForSeconds(resourceGenerationInterval);

            // 1 per basement + 0.2 per territory
            resources += 1 + MapManager.Instance.CountTerritoriesCurrentPlayer() * 0.2f; 

            UIManager.Instance.UpdateResourceDisplay(resources);
        }
    }

    public bool HasEnoughResources(int amount)
    {
        return resources >= amount;
    }

    public void DeductResources(int amount)
    {
        if (resources >= amount)
        {
            resources -= amount;
            UIManager.Instance.UpdateResourceDisplay(resources); // Update the UI
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}