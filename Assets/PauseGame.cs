using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton, settingButton, mainMenuButton; 
    private bool isPaused = false;

    private void Awake()
    {
        resumeButton.onClick.AddListener(delegate { Resume(); });
        settingButton.onClick.AddListener(delegate { Resume(); });  // TEMPORARY
        mainMenuButton.onClick.AddListener(delegate { LoadMainMenu(); });
    }
    void Update()
    {
        // ESC to Pause Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0f; // pause the game
        isPaused = true;
        pauseMenuUI.SetActive(true); // show the pause menu
    }

    public void Resume()
    {
        Time.timeScale = 1f; // resume the game
        isPaused = false;
        pauseMenuUI.SetActive(false); // hide the pause menu
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // resume the game in case it was paused
        SceneManager.LoadScene("MainMenu"); // load the main menu scene
    }

    public void QuitGame()
    {
        Application.Quit(); // quit the game
    }
}