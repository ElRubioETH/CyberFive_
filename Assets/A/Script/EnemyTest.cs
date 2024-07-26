using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestEnemy : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100; // Maximum health for the slider
    public int goldReward = 100; // Gold given when the enemy dies
    public Slider healthBar; // Reference to the health bar slider
    public Slider delayedHealthBar; // Reference to the delayed health bar slider
    public float healthBarUpdateDuration = 1.5f; // Duration for the health bar to update
    public Animator animator; // Reference to the animator
    public float attackDelay = 0.5f; // Delay before applying damage
    public float attackInterval = 3.0f; // Time between attacks if player stays in the collider
    public float moveSpeed = 2.0f; // Speed of enemy movement
    public float pointAX; // Start X position of movement
    public float pointBX; // End X position of movement
    private bool isAttacking = false;
    private bool isMoving = true;
    private Transform player; // To keep reference of player
    private float lastDamageTime; // Time when the last damage was taken
    private bool isUpdatingDelayedHealthBar = false; // Flag to check if the delayed health bar coroutine is running
    private Vector3 targetPosition; // The target position for movement
    private void Start()
    {
        // Initialize the health bar sliders
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }

        if (delayedHealthBar != null)
        {
            delayedHealthBar.maxValue = maxHealth;
            delayedHealthBar.value = health;
        }
        targetPosition = new Vector3(pointBX, transform.position.y, transform.position.z);
        StartCoroutine(MoveToPoint());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isMoving = false;
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            isAttacking = false;
            isMoving = true;
            animator.SetBool("IsMoving", true);
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        while (player != null)
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack"); // Trigger the attack animation
            }
            yield return new WaitForSeconds(attackDelay);
            if (player != null)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(20);
                }
            }
            yield return new WaitForSeconds(attackInterval - attackDelay);
        }
        isAttacking = false;
    }


    private IEnumerator MoveToPoint()
    {
        animator.SetBool("IsMoving", true);
        while (true)
        {
            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                if (transform.position == targetPosition)
                {
                    if (targetPosition.x == pointAX)
                    {
                        targetPosition = new Vector3(pointBX, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        targetPosition = new Vector3(pointAX, transform.position.y, transform.position.z);
                    }
                    Flip();
                }
                yield return null;
            }
            else
            {
                animator.SetBool("IsMoving", false);
                yield return null;
            }
        }
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }



    public void TakeDamage(float damage)
    {
        health -= (int)damage;
        lastDamageTime = Time.time;
        if (healthBar != null)
        {
            StartCoroutine(UpdateHealthBar(healthBar, healthBar.value, health, healthBarUpdateDuration));
        }
        if (!isUpdatingDelayedHealthBar && delayedHealthBar != null)
        {
            StartCoroutine(UpdateDelayedHealthBar());
        }
        if (health <= 0)
        {
            Die();
        }
    }


    private IEnumerator UpdateHealthBar(Slider slider, float startValue, float endValue, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, endValue, elapsed / duration);
            yield return null;
        }
        slider.value = endValue;
    }

    private IEnumerator UpdateDelayedHealthBar()
    {
        isUpdatingDelayedHealthBar = true;
        while (Time.time - lastDamageTime < 1.0f)
        {
            yield return null;
        }

        if (delayedHealthBar != null)
        {
            StartCoroutine(UpdateHealthBar(delayedHealthBar, delayedHealthBar.value, health, healthBarUpdateDuration));
        }

        isUpdatingDelayedHealthBar = false;
    }

    void Die()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.AddGold(goldReward);
        }
        Destroy(gameObject);
    }
}
