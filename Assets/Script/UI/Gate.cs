using UnityEngine;
using UnityEngine.SceneManagement;

public class GateController : MonoBehaviour
{
    public Animator gateAnimator; // Reference to the Animator component
    public string sceneName; // The name of the scene to load
    private bool isPlayerInTrigger = false; // To check if player is in the trigger

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object entering is the player
        {
            isPlayerInTrigger = true;
            gateAnimator.SetBool("isOpen", true); // Play the open animation
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object exiting is the player
        {
            isPlayerInTrigger = false;
            gateAnimator.SetBool("isOpen", false); // Play the close animation
        }
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.L)) // Check if player is in trigger and L key is pressed
        {
            SceneManager.LoadScene(sceneName); // Load the specified scene
        }
    }
}
