using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mission1 : MonoBehaviour
{

    public InputField passwordField;
    public Button submitButton;
    public TextMeshProUGUI feedbackText;
    public GameObject door;
    public Animator trap;
    public GameObject pass;

    private string correctPassword = "12345"; // Thay đổi mật khẩu đúng tại đây

    void Start()
    {
        //submitButton.onClick.AddListener(CheckPassword);
        door.SetActive(true); // Đảm bảo cửa đang đóng khi bắt đầu
        trap.SetBool("trap", true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pass.SetActive(true);
            CheckPassword();
        }
    }

    void CheckPassword()
    {
        if (passwordField.text == correctPassword)
        {
            feedbackText.text = "Correct Password!";
            OpenDoor();
            Closetrap();
        }
        else
        {
            feedbackText.text = "Wrong Password. Try Again!";
        }
    }

    void OpenDoor()
    {
        // Ẩn cửa hoặc thực hiện hành động mở cửa tại đây
        door.SetActive(false); // Giả sử cửa biến mất khi mở
    }
    void Closetrap()
    {
        trap.SetBool("trap", false);
    }
}
