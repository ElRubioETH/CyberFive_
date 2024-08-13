using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BHdialogue : MonoBehaviour
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
        "P: Mày nghĩ cái cơ thể to lớn đó sẽ cứu được mày sao? Trong thế giới này, sức mạnh không nằm ở cơ bắp mà ở những gì mày biết.",
        "N: Tao không cần thông minh để đè bẹp mày. Chỉ cần một cú đấm là mày đã nằm xuống rồi.",
        "P: Mày có thể nghiền nát tao, nhưng đừng quên, tao là người duy nhất có thể đưa mày đến nơi mày muốn.",
        "N: Mày nghĩ mình là ai mà dám đứng trước mặt tao như thế này? Mày chỉ là một hạt cát trong sa mạc mà thôi.",
        "P: Thế giới này không dành cho kẻ yếu đuối, nhưng cũng không dành cho những thằng to xác mà không có não đâu.",
        "N: Mày có thể đánh bại tao, nhưng mày không thể chạy trốn khỏi chính mình. Những bóng tối mày tạo ra cuối cùng sẽ nuốt chửng mày",
        "P: Công nghệ của mày có thể nhanh, nhưng tao thì vẫn còn nhanh hơn thế.",
        "N: Mày có thể chạy, nhưng mày không thể trốn mãi được. Tao sẽ tìm ra mày, và khi đó sẽ không có nơi nào để mày ẩn náu."
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
        SceneManager.LoadScene("1ndboss");
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