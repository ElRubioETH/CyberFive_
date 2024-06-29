using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Button resolution1080pButton;
    public Button resolution720pButton;
    public Button playButton;
    public Button settingsButton;
    public Button returnButton;
    public Button exitButton;

    void Start()
    {
        // Set up button listeners
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        returnButton.onClick.AddListener(ReturnToMainMenu);
        exitButton.onClick.AddListener(ExitGame);
        resolution1080pButton.onClick.AddListener(() => SetResolution(1920, 1080));
        resolution720pButton.onClick.AddListener(() => SetResolution(1366, 768));

        // Set up volume slider listener
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        // Initialize settings
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    void ReturnToMainMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void SetResolution(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
