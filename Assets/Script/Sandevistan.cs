using UnityEngine;
using System.Collections;

public class SandevistanSkill : MonoBehaviour
{
    public SpriteGhostTrailRenderer spriteGhostTrailRenderer; // Reference to the ghost trail renderer
    public float cooldown = 5f; // Cooldown in seconds
    public float duration = 10f; // Duration in seconds
    public float slowMotionFactor = 0.1f; // Factor to slow down time
    public float playerSpeedMultiplier = 2f; // Speed multiplier for the player
    private float skillTimer;
    private bool skillActive = false;
    private float originalRunSpeed;
    private float originalWalkSpeed;
    private float originalAnimSpeed;
    public GameObject sandevistanbg;
    public AudioSource sandevistansf;
    private PlayerController playerController;
    private Animator playerAnimator;

    private void Start()
    {
        spriteGhostTrailRenderer.enabled = false;
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();

        if (playerController != null)
        {
            originalRunSpeed = playerController.runSpeed;
            originalWalkSpeed = playerController.walkSpeed;
        }
        else
        {
            Debug.LogError("PlayerController not found on the GameObject.");
        }

        if (playerAnimator != null)
        {
            originalAnimSpeed = playerAnimator.speed;
        }
        else
        {
            Debug.LogError("Animator not found on the GameObject.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !skillActive) // Activate skill with 'E' key
        {
            StartCoroutine(ActivateSandevistan());
            sandevistanbg.SetActive(true);
            sandevistansf.Play();
        }

        if (skillActive)
        {
            skillTimer -= Time.unscaledDeltaTime; // Use unscaledDeltaTime to properly account for slow-motion
            if (skillTimer <= 0)
            {

                DeactivateSandevistan();
                sandevistanbg.SetActive(false);

            }
        }
    }

    private IEnumerator ActivateSandevistan()
    {
        skillActive = true;
        skillTimer = duration;

        // Enable ghost trail
        spriteGhostTrailRenderer.enabled = true;

        // Slow down time
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Speed up player
        playerController.runSpeed *= playerSpeedMultiplier;
        playerController.walkSpeed *= playerSpeedMultiplier;

        // Speed up animations
        if (playerAnimator != null)
        {
            playerAnimator.speed *= playerSpeedMultiplier;
        }

        // Wait for the duration of the skill
        yield return new WaitForSecondsRealtime(duration);

        DeactivateSandevistan();

        // Cooldown
        yield return new WaitForSecondsRealtime(cooldown);

        skillActive = false;
    }

    private void DeactivateSandevistan()
    {
        // Restore normal time
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Restore player speed
        playerController.runSpeed = originalRunSpeed;
        playerController.walkSpeed = originalWalkSpeed;

        // Restore animation speed
        if (playerAnimator != null)
        {
            playerAnimator.speed = originalAnimSpeed;
        }

        // Disable ghost trail
        spriteGhostTrailRenderer.enabled = false;
    }
}
