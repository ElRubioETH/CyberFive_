using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Vector2[] positions; // Array of positions as Vector2 values
    public GameObject[] warningImages; // Array of warning images corresponding to each trap position
    private float activationInterval = 2f;
    private float activeDuration = 1f;
    private bool isActive = false;
    private SpriteRenderer spriteRenderer; // To control the trap's visibility
    private int currentTrapIndex;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(TrapRoutine());
    }

    private IEnumerator TrapRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(activationInterval - 2f);

            // Show the warning image 1 second before the trap activates
            ShowWarning();

            // Wait 1 second before activating the trap
            yield return new WaitForSeconds(2f);

            // Activate the trap
            ActivateTrap();

            // Wait for the active duration
            yield return new WaitForSeconds(activeDuration);

            // Deactivate the trap (but keep the GameObject active)
            DeactivateTrap();
        }
    }

    private void ShowWarning()
    {
        // Choose a random position and corresponding warning image from the array
        currentTrapIndex = Random.Range(0, positions.Length);
        Vector2 selectedPosition = positions[currentTrapIndex];
        transform.position = selectedPosition;

        // Set the corresponding warning image to active
        warningImages[currentTrapIndex].SetActive(true);
    }

    private void ActivateTrap()
    {
        // Set the warning image to inactive
        warningImages[currentTrapIndex].SetActive(false);

        // Set the trap to active (visible and able to deal damage)
        isActive = true;
        spriteRenderer.enabled = true;
    }

    private void DeactivateTrap()
    {
        // Set the trap to inactive (invisible and unable to deal damage)
        isActive = false;
        spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.CompareTag("Player"))
        {
            // Assuming you have a Player script with a TakeDamage method
            collision.GetComponent<PlayerController>().TakeDamage(1); // Adjust the damage value as needed
        }
    }
}
