using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneOnClick : MonoBehaviour
{
    // Assign this in the Unity Inspector
    public Button yourButton;
    public string sceneName;

    void Start()
    {
        // Add the listener to the button
        yourButton.onClick.AddListener(LoadScene);
    }

    void LoadScene()
    {
        // Load the specified scene
        SceneManager.LoadScene("FinalBoss");
    }
}
