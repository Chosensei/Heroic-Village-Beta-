using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIController : Singleton<MenuUIController>
{
    public GameObject pauseMenuUI, SettingMenuUI;
    public Button resumeButton, settingButton, mainMenuButton;
    public Button toggleMusic, toggleSfx;
    public Slider musicSlider, sfxSlider;
    private bool isPaused = false;
    private void Awake()
    {
        // Generals
        resumeButton.onClick.AddListener(() => { Resume(); });
        settingButton.onClick.AddListener(() => { OpenSettingMenu(); });  
        mainMenuButton.onClick.AddListener(() => { LoadMainMenu(); });
        // Audio 
        toggleMusic.onClick.AddListener(()=> { ToggleMusic(); });
        toggleSfx.onClick.AddListener(() => { ToggleSfx(); });
        musicSlider.onValueChanged.AddListener((value) => { MusicVolume(value); });
        sfxSlider.onValueChanged.AddListener((value) => { SfxVolume(value); });
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

    public void Pause()
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
        SettingMenuUI.SetActive(false); // hide the settings menu
    }
    public void OpenSettingMenu()
    {
        SettingMenuUI.SetActive(true); 
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
    public void ToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();
    }
    public void ToggleSfx()
    {
        SoundManager.Instance.ToggleSfx();
    }
    public void MusicVolume(float volume)
    {
        SoundManager.Instance.MusicVolume(musicSlider.value);
    }
    public void SfxVolume(float volume)
    {
        SoundManager.Instance.SfxVolume(sfxSlider.value);
    }
}
