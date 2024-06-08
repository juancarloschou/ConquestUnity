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

    private float resources;
    private float resourceGenerationInterval = 1f; // Interval in seconds for resource generation

    private float resourcesPerTerritory = 0.2f;
    private float resourcesPerBasement = 1f;

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

            UIManager.Instance.UpdateResourceDisplay(resources); // Initialize resource display

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
            resources += MapManager.Instance.CountBasementsCurrentPlayer() * resourcesPerBasement + 
                MapManager.Instance.CountTerritoriesCurrentPlayer() * resourcesPerTerritory; 

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