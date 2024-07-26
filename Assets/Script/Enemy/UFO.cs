using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UFOEnemy : MonoBehaviour
{
    // UFO movement variables
    public float pointA = -5f;  // X-coordinate of point A
    public float pointB = 5f;   // X-coordinate of point B
    public float speed = 2.0f;  // Tốc độ di chuyển của UFO

    // Health and damage variables
    public int health = 100;
    public int maxHealth = 100; // Maximum health for the slider
    public int goldReward = 100; // Gold given when the enemy dies
    public int damageFromProjectile = 50; // Damage taken from player projectiles
    public Slider healthBar; // Reference to the health bar slider
    public Slider delayedHealthBar; // Reference to the delayed health bar slider
    public float healthBarUpdateDuration = 1.5f; // Duration for the health bar to update

    private float currentTarget;  // X-coordinate of the current target
    private Rigidbody2D rb;  // Tham chiếu đến Rigidbody2D của UFO
    private SpriteRenderer spriteRenderer;  // Tham chiếu đến SpriteRenderer của UFO
    private float lastDamageTime; // Time when the last damage was taken
    private bool isUpdatingDelayedHealthBar = false; // Flag to check if the delayed health bar coroutine is running

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTarget = pointB;  // Ban đầu, di chuyển tới điểm B

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
        MoveToTarget();
        CheckSwitchTarget();
    }

    void MoveToTarget()
    {
        // Di chuyển UFO tới điểm hiện tại
        float step = speed * Time.deltaTime;
        transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, currentTarget, step), transform.position.y);
    }

    void CheckSwitchTarget()
    {
        // Nếu UFO gần điểm hiện tại, chuyển đổi mục tiêu
        if (Mathf.Abs(transform.position.x - currentTarget) < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            spriteRenderer.flipX = !spriteRenderer.flipX;  // Quay đầu lại
        }
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
