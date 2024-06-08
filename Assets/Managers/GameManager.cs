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
    public static GameManager Instance { get; private set; }

    // Default selected options
    private OptionsMenuController.GameType defaultGameType = OptionsMenuController.GameType.Local;
    private int defaultPlayerQuantity = 2;
    private Color defaultPlayerColor = Color.blue;

    // Selected options properties
    private OptionsMenuController.GameType selectedGameType;
    private int selectedPlayerQuantity;
    private Color selectedPlayerColor;

    // Current player and color of each player
    public Color[] playerColors; // Array to store the colors of each player (index based 0)
    public int currentPlayer; // Index to track the current player (starting by 1)
    public bool alive; // if game is ongoing (no game over)

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
        SetDefaultOptions(); // Set default options on Awake
    }

    // Start is called before the first frame update
    void Start()
    {
        alive = false;
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

        // What player number is the current player
        currentPlayer = 1;

        // game ongoing, player alive
        alive = true;

        // Initialize playerColors array based on selectedPlayerQuantity
        playerColors = new Color[selectedPlayerQuantity];

        // Assign selected player's color to the first index
        playerColors[0] = selectedPlayerColor;

        // Initialize playerColors array based on selectedPlayerQuantity and selectedPlayerColor
        // select the color of the oponents
        for (int i = 0; i < selectedPlayerQuantity; i++)
        {
            // the current player has his color, we need to find color of opponents
            if (i != currentPlayer - 1)
            {
                playerColors[i] = GetColorForIndex(i);
                if(playerColors[i] == selectedPlayerColor)
                {
                    // if the opponent get the color of player, get the other color avaliable
                    playerColors[i] = GetColorForIndex(GetIndexForColor(selectedPlayerColor));
                }
            }
        }

        for (int i = 0; i < selectedPlayerQuantity; i++)
        {
            Debug.Log($"Player {i} color: {playerColors[i]}");
        }

        // For demonstration purposes, let's print the selected options
        Debug.Log($"Starting the game with GameType: {selectedGameType}, Player Quantity: {selectedPlayerQuantity}, Player Color: {selectedPlayerColor}");

        // initialize game other managers
        ResourceManager.Instance.InitializeGame();
        MapManager.Instance.InitializeGame();
        TerritoryManager.Instance.InitializeGame();
        TroopManager.Instance.InitializeGame();
        UIManager.Instance.InitializeGame();

        // Load the GameScene
        SceneManager.LoadScene("GameScene");
    }

    // Method to get a Color for a given dropdown index
    public Color GetColorForIndex(int index)
    {
        switch (index)
        {
            case 0:
                return Color.blue;
            case 1:
                return Color.red;
            case 2:
                return Color.green;
            case 3:
                return Color.yellow;
            default:
                return Color.blue; // Default color
        }
    }

    // Method to get a index for a given Color
    public int GetIndexForColor(Color color)
    {
        // Map the color to a dropdown index (customize based on your color choices)
        if (color == Color.blue)
            return 0;
        else if (color == Color.red)
            return 1;
        else if (color == Color.green)
                    return 2;
        else if (color == Color.yellow)
            return 3;
        else
            return 0; // Default index
    }

    // Method to get the color of the player
    public Color GetPlayerColor(int player)
    {
        return playerColors[player - 1]; // Adjusted to index from 0
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
