using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorControllerlv2 : MonoBehaviour
{
    public Animator doorAnimator; // Reference to the Animator component
    public string sceneToLoad; // Name of the scene to load
    private bool isPlayerNear = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            doorAnimator.SetBool("isOpen", true);
            doorAnimator.SetBool("isIdle", false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            doorAnimator.SetBool("isOpen", false);
            doorAnimator.SetBool("isIdle", false);
            // Wait for the close animation to finish before setting idle animation
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(WaitForCloseAnimation());
            }
        }
    }

    private IEnumerator WaitForCloseAnimation()
    {
        // Assuming the close animation length is 1 second
        yield return new WaitForSeconds(1f);
        if (!isPlayerNear)
        {
            doorAnimator.SetBool("isIdle", true);
        }
    }

    void Update()
    {
        if (isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadScene();
            }
        }
        else
        {
            doorAnimator.SetBool("isIdle", true);
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene("hehe");
    }
}
