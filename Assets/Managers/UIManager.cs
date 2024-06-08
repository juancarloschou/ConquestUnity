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

    // For drawing the resources icon
    public Sprite resourceSprite; // Asign the sprite of the resource

    // train action buttons
    public Button createKnightButton;

    // movement action buttons
    public GameObject movementOptionsPanel;
    public Button cancelSelectionButton;
    private int quantityMovement;

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
        HideButtonCreateKnight();
        HideMovementOptions();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            resourceText = GameObject.Find("TextResources")?.GetComponent<TextMeshProUGUI>();
            if(resourceText == null) 
            {
                Debug.LogError("Resource icon not found");
            }

            resourceIcon = GameObject.Find("ImageResources")?.GetComponent<Image>();
            if (resourceIcon != null)
            {
                resourceIcon.sprite = resourceSprite;
            }
            else
            {
                Debug.LogError("Resource icon not found");
            }

            createKnightButton = GameObject.Find("ButtonCreateKnight")?.GetComponent<Button>();
            if (createKnightButton != null)
            {
                createKnightButton.onClick.AddListener(() => TroopManager.Instance.CreateTroopKnight());
                HideButtonCreateKnight();
            }
            else
            {
                Debug.LogError("createKnightButton not found");
            }

            // Find and assign the UI elements for movement
            movementOptionsPanel = GameObject.Find("PanelMovementOptions");
            if (movementOptionsPanel != null)
            {
                Debug.Log("movementOptionsPanel found");
                //HideMovementOptions();
            }
            else
            {
                Debug.LogError("PanelMovementOptions not found.");
            }

            cancelSelectionButton = GameObject.Find("PanelMovementOptions/ButtonCancelSelection")?.GetComponent<Button>();
            if (cancelSelectionButton != null)
            {
                Debug.Log("cancelSelectionButton found");
                cancelSelectionButton.onClick.AddListener(() => TroopManager.Instance.CancelSelection());
            }
            else
            {
                Debug.LogError("cancelSelectionButton not found.");
            }

            // the panel needs to be active to find the button cancel, now I can hide it
            HideMovementOptions();
        }
        else
        {
            createKnightButton?.onClick.RemoveAllListeners();
            cancelSelectionButton?.onClick.RemoveAllListeners();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        createKnightButton?.onClick.RemoveAllListeners();
        cancelSelectionButton?.onClick.RemoveAllListeners();

    }

    /*
    // show for first time, add sprinte and show resources
    public void ShowResources(float resources, Sprite icon)
    {
        if (resourceIcon != null)
        {
            resourceIcon.sprite = icon;
        }
        else
        {
            Debug.LogError("Resource icon not found");
        }

        UpdateResourceDisplay(resources);
    }
    */

    public void UpdateResourceDisplay(float resources)
    {
        int roundedResources = Mathf.RoundToInt(resources);
        resourceText.text = roundedResources.ToString();
    }

    public void HideButtonCreateKnight()
    {
        if (createKnightButton != null)
        {
            createKnightButton?.gameObject.SetActive(false);
        }
    }

    public void ShowButtonCreateKnight()
    {
        createKnightButton.gameObject.SetActive(true);
    }

    public void ShowMovementOptions(int defendersStrength)
    {
        // by default all the soldiers
        quantityMovement = defendersStrength;

        if (quantityMovement > 1)
        {
            // Position and display the movement options panel
            movementOptionsPanel?.SetActive(true);
        }
    }

    public void HideMovementOptions()
    {
        if (movementOptionsPanel != null)
        {
            movementOptionsPanel?.SetActive(false);
        }
    }

    //public void ShowCancelSelectionButton()
    //{
    //    cancelSelectionButton.gameObject.SetActive(true);
    //}

    //public void HideCancelSelectionButton()
    //{
    //    cancelSelectionButton.gameObject.SetActive(false);
    //}

    /*
    public void OnCanvasGroupChanged()
    {
        // when the troops are selected the user can cancel the options of movement and deselect
        TroopManager.Instance.DeselectTroop();

        HideMovementOptions();
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
