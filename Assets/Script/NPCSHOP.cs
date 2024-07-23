using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject shopPanel; // Reference to the shop panel UI
    public TextMeshProUGUI interactionPrompt; // Reference to the TextMeshPro UI element
    public string interactionKey = "f"; // Key to interact with the NPC
    private bool playerInRange = false; // Flag to check if the player is in range

    private void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false); // Hide the prompt initially
        }
        else
        {
            Debug.LogError("Interaction Prompt is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(true); // Show the prompt when the player enters the trigger
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false); // Hide the prompt when the player exits the trigger
            }
        }
    }

    private void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Shop Panel is not assigned in the inspector.");
        }
    }
}
