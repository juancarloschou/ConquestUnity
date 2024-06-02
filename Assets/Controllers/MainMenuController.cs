using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlayGame()
    {
        // Ensure the GameManager is found before starting the game
        if (GameManager.Instance != null)
        {
            // Set selected options in GameManager before starting the game
            GameManager.Instance.StartGame();
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

