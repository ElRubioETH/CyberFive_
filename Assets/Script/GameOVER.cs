using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;  // The Game Over panel to activate
    public GameObject sceneChoosePanel;  // The Scene Choose panel to activate

    void Start()
    {
        gameOverPanel.SetActive(false);
        sceneChoosePanel.SetActive(false);
    }

    // Call this method when the player dies
    public void OnPlayerDeath()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;  // Pause the game
    }

    // Button function to restart the current scene
    public void RestartScene()
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Button function to go to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene("MainMenu");  // Replace with your Main Menu scene name
    }

    // Button function to open the Scene Choose panel
    public void OpenSceneChoosePanel()
    {
        gameOverPanel.SetActive(false);
        sceneChoosePanel.SetActive(true);
    }

    // Button function to load a specific scene
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene(sceneName);
    }
    public void LoadLv1()
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene("lv1");  // Replace with your Main Menu scene name
    }
    public void LoadLv2()
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene("lv2");  // Replace with your Main Menu scene name
    }
    public void LoadLv3()
    {
        Time.timeScale = 1f;  // Unpause the game
        SceneManager.LoadScene("hehe");  // Replace with your Main Menu scene name
    }
}
