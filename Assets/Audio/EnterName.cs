using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EnterPlayerNamePanel : MonoBehaviour
{
    public GameObject enterNamePanel;
    public TMP_InputField playerNameInputField;
    public TMP_Text errorMessageText;
    public Button confirmButton;

    private void Start()
    {
        // Initialize panel and error message
        enterNamePanel.SetActive(false);
        errorMessageText.gameObject.SetActive(false);

        // Add listener to confirm button
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    public void ShowPanel()
    {
        enterNamePanel.SetActive(true);
    }

    private void OnConfirmButtonClicked()
    {
        string playerName = playerNameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            StartCoroutine(ShowErrorMessage("Invalid name"));
        }
        else
        {
            // Save the player name
            PlayerPrefs.SetString("PlayerName", playerName);

            // Load the next scene or perform any action you need
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        errorMessageText.gameObject.SetActive(false);
    }
}
