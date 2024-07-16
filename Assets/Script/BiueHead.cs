using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlueHead : MonoBehaviour
{
    // UFO movement variables
    public float pointA = -5f;  // X-coordinate of point A
    public float pointB = 5f;   // X-coordinate of point B
    public float speed = 2.0f;  // UFO movement speed

    // Health and damage variables
    public int health = 100;
    public int maxHealth = 100; // Maximum health for the slider
    public int goldReward = 100; // Gold given when the enemy dies
    public int damageFromProjectile = 50; // Damage taken from player projectiles
    public Slider healthBar; // Reference to the health bar slider
    public Slider delayedHealthBar; // Reference to the delayed health bar slider
    public float healthBarUpdateDuration = 1.5f; // Duration for the health bar to update

    private float currentTarget;  // X-coordinate of the current target
    private Rigidbody2D rb;  // Reference to the UFO's Rigidbody2D
    private SpriteRenderer spriteRenderer;  // Reference to the UFO's SpriteRenderer
    private Animator animator; // Reference to the UFO's Animator
    private float lastDamageTime; // Time when the last damage was taken
    private bool isUpdatingDelayedHealthBar = false; // Flag to check if the delayed health bar coroutine is running
    private bool isStandingStill = false; // Flag to check if the UFO is standing still

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentTarget = pointB;  // Initially, move to point B

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
    }

    void Update()
    {
        if (!isStandingStill)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        // Determine direction and flip sprite if necessary
        float direction = currentTarget > transform.position.x ? 1f : -1f;
        spriteRenderer.flipX = direction < 0f;

        // Move the UFO towards the current target
        float step = speed * Time.deltaTime;
        transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, currentTarget, step), transform.position.y);

        // Play run animation while moving
        animator.SetBool("IsRunning", true);

        // If the UFO is close to the current target, switch target and stand still for 3 seconds
        if (Mathf.Abs(transform.position.x - currentTarget) < 0.1f)
        {
            StartCoroutine(StandStill());
        }
    }

    IEnumerator StandStill()
    {
        // Stop moving and play idle animation
        isStandingStill = true;
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsIdle", true);

        // Stand still for 3 seconds
        yield return new WaitForSeconds(3f);

        // Resume moving
        animator.SetBool("IsIdle", false);
        isStandingStill = false;
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(10); // Example damage value
            }
        }
        else if (other.CompareTag("Projectile"))
        {
            TakeDamage(damageFromProjectile);
            Destroy(other.gameObject); // Destroy the projectile on hit
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
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
