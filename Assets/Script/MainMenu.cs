using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro; // Add this for TextMeshPro

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
    public AudioMixer audioMixer; // Reference to the Audio Mixer
    public TMP_Text volumePercentageText; // Reference to TMP text for volume percentage

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

        // Set the slider's initial value to match the current volume
        float currentVolume;
        audioMixer.GetFloat("MasterVolume", out currentVolume);
        volumeSlider.value = Mathf.Pow(10, currentVolume / 20); // Convert dB to linear value

        // Update volume percentage text
        UpdateVolumePercentageText(volumeSlider.value);
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
        Debug.Log($"Resolution set to: {width}x{height}, FullScreen: {Screen.fullScreen}");

        // Force canvas update to adjust UI elements
        Canvas.ForceUpdateCanvases();
    }

    void ChangeVolume(float volume)
    {
        // Prevent the volume from being set to 0 to avoid logarithm issues
        float minVolume = 0.0001f; // Smallest volume to use when slider is at 0
        float clampedVolume = Mathf.Clamp(volume, minVolume, 1f);

        // Convert linear value to dB
        float dB = Mathf.Log10(clampedVolume) * 20;

        audioMixer.SetFloat("MasterVolume", dB);

        // Update volume percentage text
        UpdateVolumePercentageText(volume);
    }

    void UpdateVolumePercentageText(float volume)
    {
        int volumePercentage = Mathf.RoundToInt(volume * 100);
        volumePercentageText.text = $"{volumePercentage}%";
    }
}
