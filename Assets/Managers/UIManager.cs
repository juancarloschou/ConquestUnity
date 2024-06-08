using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// - Controls UI elements, such as resource displays and territory information.
/// Updates troop quantities, resource indicators, and other UI elements.
/// Receives information from relevant managers (e.g., TroopManager, ResourceManager).
/// - Manages transitions between different screens.
/// Controls the activation and deactivation of UI elements based on the active screen.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // show resources quantity
    public TextMeshProUGUI resourceText; // Assign this from the inspector
    public Image resourceIcon; // Assign this from the inspector

    // action buttons
    public Button createKnightButton;

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
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            resourceText = GameObject.Find("TextResources").GetComponent<TextMeshProUGUI>();
            resourceIcon = GameObject.Find("ImageResources").GetComponent<Image>();
            createKnightButton = GameObject.Find("ButtonCreateKnight").GetComponent<Button>();
            createKnightButton.onClick.AddListener(() => TroopManager.Instance.CreateTroopKnight());
            HideButtonCreateKnight();
        }
        else
        {
            if (createKnightButton != null)
            {
                createKnightButton.onClick.RemoveAllListeners();
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void HideButtonCreateKnight()
    {
        //createKnightButton.enabled = false;
        createKnightButton.gameObject.SetActive(false);
    }

    public void ShowButtonCreateKnight()
    {
        //createKnightButton.enabled = true;
        createKnightButton.gameObject.SetActive(true);
    }

    public void ShowResources(float resources, Sprite icon)
    {
        int roundedResources = Mathf.RoundToInt(resources);
        resourceText.text = roundedResources.ToString();

        if (resourceIcon != null)
        {
            resourceIcon.sprite = icon;
            resourceIcon.enabled = true;
        }
        else
        {
            Debug.LogError("Resource icon not found");
        }
    }

    public void UpdateResourceDisplay(float resources)
    {
        int roundedResources = Mathf.RoundToInt(resources);
        resourceText.text = roundedResources.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
