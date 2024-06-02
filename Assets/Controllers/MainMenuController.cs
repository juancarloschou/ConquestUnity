using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Reference to the GameManager
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void PlayGame()
    {
        // Ensure the GameManager is found before starting the game
        if (gameManager != null)
        {
            // Set selected options in GameManager before starting the game
            gameManager.StartGame();
        }
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenuScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

