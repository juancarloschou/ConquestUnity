using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    private TMP_Dropdown gameTypeDropdown;
    private TMP_Dropdown playerQuantityDropdown;
    private TMP_Dropdown playerColorDropdown;

    // Enum to represent the game type
    public enum GameType
    {
        Local,
        Network
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get references to Dropdown components
        gameTypeDropdown = GetDropdownWithName("cmbGameType");
        playerQuantityDropdown = GetDropdownWithName("cmbPlayerQuantity");
        playerColorDropdown = GetDropdownWithName("cmbPlayerColor");

        // Ensure Dropdowns are not null before using them
        if (gameTypeDropdown == null || playerQuantityDropdown == null || playerColorDropdown == null)
        {
            Debug.LogError("Dropdowns not found. Please check the names or structure.");
            return;
        }

        // Add listener methods to the dropdowns
        gameTypeDropdown.onValueChanged.AddListener(OnGameTypeChanged);
        playerQuantityDropdown.onValueChanged.AddListener(OnPlayerQuantityChanged);
        playerColorDropdown.onValueChanged.AddListener(OnPlayerColorChanged);

        // Populate color dropdown with color options
        PopulateColorDropdown();

        // Set dropdown values from the GameManager
        SetValuesFromGameManager();
    }

    // Method to get a TMP_Dropdown by name
    private TMP_Dropdown GetDropdownWithName(string dropdownName)
    {
        Transform dropdownTransform = transform.Find("OptionsMenuPanel/" + dropdownName);
        if (dropdownTransform != null)
        {
            return dropdownTransform.GetComponent<TMP_Dropdown>();
        }
        else
        {
            Debug.LogError($"Dropdown named {dropdownName} not found. Please check the names or structure.");
            return null;
        }
    }

    // Method to populate the color dropdown
    void PopulateColorDropdown()
    {
        // Add color options to the dropdown
        // (You can customize this based on your color choices)
        List<string> colorOptions = new List<string> { "Blue", "Green", "Red", "Yellow" };
        playerColorDropdown.ClearOptions();
        playerColorDropdown.AddOptions(colorOptions);
    }

    // Event handler for game type dropdown change
    void OnGameTypeChanged(int value)
    {
        GameType selectedGameType = (GameType)value;
        GameManager.Instance.SetSelectedGameType(selectedGameType);
    }

    // Event handler for player quantity dropdown change
    void OnPlayerQuantityChanged(int value)
    {
        int selectedPlayerQuantity = value + 2; // Add 2 to convert dropdown index to quantity (assuming minimum is 2 player)
        GameManager.Instance.SetSelectedPlayerQuantity(selectedPlayerQuantity);
    }

    // Event handler for player color dropdown change
    void OnPlayerColorChanged(int value)
    {
        // Map the dropdown index to a color (customize based on your color choices)
        Color selectedPlayerColor = GetColorForIndex(value);
        GameManager.Instance.SetSelectedPlayerColor(selectedPlayerColor);
    }

    // Method to get a Color for a given dropdown index
    Color GetColorForIndex(int index)
    {
        switch (index)
        {
            case 0:
                return Color.blue;
            case 1:
                return Color.green;
            case 2:
                return Color.red;
            case 3:
                return Color.yellow;
            default:
                return Color.blue; // Default color
        }
    }

    // Method to get a index for a given Color
    int GetIndexForColor(Color color)
    {
        // Map the color to a dropdown index (customize based on your color choices)
        if (color == Color.blue)
            return 0;
        else if (color == Color.green)
            return 1;
        else if (color == Color.red)
            return 2;
        else if (color == Color.yellow)
            return 3;
        else
            return 0; // Default index
    }

    // Method to set initial dropdown values based on GameManager
    void SetValuesFromGameManager()
    {
        // Set initial dropdown values based on GameManager values
        gameTypeDropdown.value = (int)GameManager.Instance.GetSelectedGameType();
        playerQuantityDropdown.value = GameManager.Instance.GetSelectedPlayerQuantity() - 2; // Subtract 2 to convert quantity to dropdown index (assuming minimum is 2 player)
        playerColorDropdown.value = GetIndexForColor(GameManager.Instance.GetSelectedPlayerColor());
        Debug.Log($"SetDropdownValuesFromGameManager. Game Type: {gameTypeDropdown.value}, Player Quantity: {playerQuantityDropdown.value}, Player Color: {playerColorDropdown.value}");
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
