using TMPro;
using UnityEngine;

public class NPCDialogue1 : MonoBehaviour
{
    public GameObject talkButton; // Reference to the "Press F to talk" UI button
    public GameObject dialogueBox; // Reference to the dialogue box UI panel
    public TMP_Text dialogueText; // Reference to the TMP_Text component inside the dialogue box

    private bool isPlayerNear = false;
    private bool isTalking = false;
    private int dialogueIndex = 0;
    private string[] dialogueLines = {
        "Chào Mừng Đến Với Thế Kỷ 25",

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
            dialogueText.text = dialogueLines[dialogueIndex]; // Update the dialogue text
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
