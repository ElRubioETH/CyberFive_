using TMPro;
using UnityEngine;

public class M_9_lv2 : MonoBehaviour
{
    public GameObject talkButton; // Reference to the "Press F to talk" UI button
    public GameObject dialogueBox; // Reference to the dialogue box UI panel
    public TMP_Text dialogueText; // Reference to the TMP_Text component inside the dialogue box
    public string npcName = "Default NPC"; // Enter the NPC name in the inspector or set it manually

    private bool isPlayerNear = false;
    private bool isTalking = false;
    private int dialogueIndex = 0;

    // Use "P" for player and "N" for NPC to identify the speaker
    private string[] dialogueLines = {
        "N: Chúc mừng cậu đã lấy được chíp ",
        "N: Để có thể lấy lõi năng lượng, trước mắt cậu phải đánh bại những kẻ hung bạo ở đây ",
        "P: Được thôi ",
        "N: Chỉ có 3 kẻ sk8 ở đây thôi, CHÚC CẬU MAY MẮN!",
        /*"N: Tốt. Nghe này, có một nhóm gọi là Bóng Ma Xanh. Chúng vừa nhận được lô hàng chíp mới từ biên giới phía Bắc. Tôi cần cậu xâm nhập vào căn cứ của chúng và lấy cho tôi một cái chíp mẫu.",
        "P: Chuyện này sẽ khó khăn đấy. Chúng có vũ khí và an ninh mạnh mẽ.",
        "N: Không ai nói đây là công việc dễ dàng. Nhưng nếu cậu làm được, tôi sẽ cung cấp cho cậu mọi thông tin về hoạt động buôn bán của chúng. Và có thể, một phần thưởng thêm.",
        "P: Được thôi",
        "N: Tốt. Đây là địa chỉ của chúng. Hãy cẩn thận, chúng không ngại giết người để bảo vệ tài sản của mình.",
        "N: chúc may mắn, kẻ săn mồi. Thành phố này không có chỗ cho kẻ yếu đuối."*/
    };

    void Start()
    {
        // Initially hide the talk button and dialogue box
        talkButton.SetActive(false);
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        // Start the dialogue when the player presses F
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            if (!isTalking)
            {
                StartDialogue();
            }
        }

        // Continue to the next line of dialogue when the player clicks the mouse button
        if (isTalking && Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialogueIndex = 0;
        dialogueBox.SetActive(true); // Show the dialogue box
        DisplayNextLine(); // Display the first line of dialogue
    }

    void DisplayNextLine()
    {
        if (dialogueIndex < dialogueLines.Length)
        {
            string dialogueLine = dialogueLines[dialogueIndex];
            string speaker = dialogueLine.Substring(0, 1);
            string dialogue = dialogueLine.Substring(2);

            if (speaker == "P")
            {
                string playerName = PlayerPrefs.GetString("PlayerName", "Player");
                dialogueText.text = $"\"{playerName}\": {dialogue}";
            }
            else if (speaker == "N")
            {
                dialogueText.text = $"\"{npcName}\": {dialogue}";
            }

            dialogueIndex++;
        }
        else
        {
            EndDialogue(); // End the dialogue when all lines are displayed
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueText.text = ""; // Clear the dialogue text
        dialogueBox.SetActive(false); // Hide the dialogue box
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            talkButton.SetActive(true); // Show the talk button when the player is near
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
            talkButton.SetActive(false); // Hide the talk button when the player is far
            EndDialogue(); // End the dialogue if the player moves away
        }
    }
}