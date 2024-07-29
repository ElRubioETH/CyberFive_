using TMPro;
using UnityEngine;

public class NPCDialogue2 : MonoBehaviour
{
    public GameObject talkButton; // Reference to the "Press F to talk" UI button
    public GameObject dialogueBox; // Reference to the dialogue box UI panel
    public TMP_Text dialogueText; // Reference to the TMP_Text component inside the dialogue box
    public TMP_Text speakerNameText; // Reference to the TMP_Text component for speaker name

    private bool isPlayerNear = false;
    private bool isTalking = false;
    private int dialogueIndex = 0;
    private string[] dialogueLines = {
        "P: Đây là chíp mẫu. Giờ thì anh phải giữ lời hứa.",
        "N: Thế mới gọi là công việc chứ. Đưa nó đây và nghe này, thằng nhóc, tôi không thích lòng vòng.",
        "Player Name: What can I do here?",
        "NPC Name: You can explore and interact with various characters."
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
            string currentLine = dialogueLines[dialogueIndex];
            string[] parts = currentLine.Split(new string[] { ": " }, System.StringSplitOptions.None);
            if (parts.Length == 2)
            {
                speakerNameText.text = parts[0]; // Update the speaker name
                dialogueText.text = parts[1]; // Update the dialogue text
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
        speakerNameText.text = ""; // Clear the speaker name text
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
