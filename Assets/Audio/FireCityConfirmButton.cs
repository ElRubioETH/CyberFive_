using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FireCityConfirmButton : MonoBehaviour
{
    public Button confirmButton;
 
    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    // Update is called once per frame
    private void OnConfirmButtonClicked()
    {
            SceneManager.LoadScene("Scene1");  
    }
}
