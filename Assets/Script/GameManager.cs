using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject inventoryPanel;
    public GameObject shopPanel; // Reference to the shop panel
    public Slider volumeSlider;
    public GameObject player; // Reference to the player GameObject
    public TMP_Text volumePercentageText;
    private PlayerController playerController;
    private bool isMenuOpen = false;
    private bool isInventoryOpen = false;
    //private bool isShopOpen = false;
    public AudioMixer audioMixer;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        float currentVolume;
        audioMixer.GetFloat("MasterVolume", out currentVolume);
        volumeSlider.value = Mathf.Pow(10, currentVolume / 20);
        UpdateVolumePercentageText(volumeSlider.value);

        if (menuPanel != null)
        {
            GameObject buttonContainer = menuPanel.transform.Find("ButtonContainer")?.gameObject;
            if (buttonContainer != null)
            {
                AssignButtonAction(buttonContainer, "ResumeButton", ResumeGame);
                AssignButtonAction(buttonContainer, "SaveButton", SaveGame);
                AssignButtonAction(buttonContainer, "LoadButton", LoadGame);
                AssignButtonAction(buttonContainer, "SettingsButton", OpenSettings);
                AssignButtonAction(buttonContainer, "MainMenuButton", BackToMainMenu);
                AssignButtonAction(buttonContainer, "ExitButton", ExitGame);
            }
            else
            {
                Debug.LogError("ButtonContainer is not found in the Menu Panel.");
            }
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

        if (inventoryPanel != null)
        {
            for (int i = 0; i < 10; i++)
            {
                int index = i; // Capture the loop variable
                string buttonName = "WeaponButton" + (i + 1);
                AssignButtonAction(inventoryPanel, buttonName, () => SelectWeapon(index));
            }
        }
        else
        {
            Debug.LogError("Inventory Panel is not assigned in the inspector.");
        }

        if (shopPanel != null)
        {
            AssignButtonAction(shopPanel, "CloseButton", CloseShop);
        }
        else
        {
            Debug.LogError("Shop Panel is not assigned in the inspector.");
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChange);
        }
        else
        {
            Debug.LogError("Volume Slider is not assigned in the inspector.");
        }
        AssignButtonActions();
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
    private void AssignButtonActions()
    {
        Button[] buttons = inventoryPanel.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => SelectWeapon(index));
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

    private void SelectWeapon(int weaponIndex)
    {
        playerController.ChangeWeapon(weaponIndex);
    }

    private void CloseShop()
    {
        //isShopOpen = false;
        shopPanel.SetActive(false);
    }
}
