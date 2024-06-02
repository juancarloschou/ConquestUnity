using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// - Manages the overall game state, transitions between screens.
/// Manages the flow between different screens.
/// Invokes relevant functionalities in other managers.
///	- Handles game initialization and termination.
/// Initializes game parameters and managers.
/// Coordinates the setup of the initial state.
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Default selected options
    private OptionsMenuController.GameType defaultGameType = OptionsMenuController.GameType.Local;
    private int defaultPlayerQuantity = 2;
    private Color defaultPlayerColor = Color.blue;

    // Selected options properties
    private OptionsMenuController.GameType selectedGameType;
    private int selectedPlayerQuantity;
    private Color selectedPlayerColor;

    void Awake()
    {
        // Ensure only one instance exists.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This ensures the GameObject persists between scenes.
        }
        else
        {
            Destroy(gameObject);
        }

        // Set default options on Awake
        SetDefaultOptions();
    }

    // Method to set default options
    private void SetDefaultOptions()
    {
        selectedGameType = defaultGameType;
        selectedPlayerQuantity = defaultPlayerQuantity;
        selectedPlayerColor = defaultPlayerColor;
        Debug.Log($"SetDefaultOptions. Game Type: {selectedGameType}, Player Quantity: {selectedPlayerQuantity}, Player Color: {selectedPlayerColor}");
    }

    // Method to set selected game type
    public void SetSelectedGameType(OptionsMenuController.GameType gameType)
    {
        selectedGameType = gameType;
    }

    // Method to set selected player quantity
    public void SetSelectedPlayerQuantity(int quantity)
    {
        selectedPlayerQuantity = quantity;
    }

    // Method to set selected player color
    public void SetSelectedPlayerColor(Color color)
    {
        selectedPlayerColor = color;
    }

    // Method to start the game with selected options
    public void StartGame()
    {
        // Add logic to start the game with selected options
        // You can access selectedGameType, selectedPlayerQuantity, and selectedPlayerColor here
        // ...

        // For demonstration purposes, let's print the selected options
        Debug.Log($"Starting the game with GameType: {selectedGameType}, Player Quantity: {selectedPlayerQuantity}, Player Color: {selectedPlayerColor}");

        // Load the GameScene
        SceneManager.LoadScene("GameScene");

    }

    // Getter method for selected game type
    public OptionsMenuController.GameType GetSelectedGameType()
    {
        return selectedGameType;
    }

    // Getter method for selected player quantity
    public int GetSelectedPlayerQuantity()
    {
        return selectedPlayerQuantity;
    }

    // Getter method for selected player color
    public Color GetSelectedPlayerColor()
    {
        return selectedPlayerColor;
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
