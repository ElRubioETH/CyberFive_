using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject inventoryPanel;
    public Slider volumeSlider;

    private bool isMenuOpen = false;
    private bool isInventoryOpen = false;

    void Start()
    {
        if (menuPanel != null)
        {
            AssignButtonAction(menuPanel, "ResumeButton", ResumeGame);
            AssignButtonAction(menuPanel, "SaveButton", SaveGame);
            AssignButtonAction(menuPanel, "LoadButton", LoadGame);
            AssignButtonAction(menuPanel, "SettingsButton", OpenSettings);
            AssignButtonAction(menuPanel, "MainMenuButton", BackToMainMenu);
            AssignButtonAction(menuPanel, "ExitButton", ExitGame);
        }
        else
        {
            Debug.LogError("Menu Panel is not assigned in the inspector.");
        }

        if (settingsPanel != null)
        {
            AssignButtonAction(settingsPanel, "BackButton", CloseSettings);
        }
        else
        {
            Debug.LogError("Settings Panel is not assigned in the inspector.");
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChange);
        }
        else
        {
            Debug.LogError("Volume Slider is not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleInventory();
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);
        if (isMenuOpen)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            settingsPanel.SetActive(false); // Ensure settings panel is closed
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
    }

    public void ResumeGame()
    {
        isMenuOpen = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void SaveGame()
    {
        // Implement your save game logic here
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        // Implement your load game logic here
        Debug.Log("Game Loaded");
    }

    public void OpenSettings()
    {
        menuPanel.SetActive(false); // Hide menu panel
        settingsPanel.SetActive(true); // Show settings panel
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false); // Hide settings panel
        menuPanel.SetActive(true); // Show menu panel
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // Ensure the game is not paused
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnVolumeChange(float volume)
    {
        // Implement volume change logic here
        AudioListener.volume = volume;
    }

    private void AssignButtonAction(GameObject panel, string buttonName, UnityEngine.Events.UnityAction action)
    {
        Transform buttonTransform = panel.transform.Find(buttonName);
        if (buttonTransform != null)
        {
            Button button = buttonTransform.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(action);
            }
            else
            {
                Debug.LogError($"Button component not found on {buttonName}");
            }
        }
        else
        {
            Debug.LogError($"{buttonName} not found in {panel.name}");
        }
    }
}
