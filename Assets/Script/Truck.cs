using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyTruck : MonoBehaviour
{
    // Health and damage variables
    public int health = 100;
    public int maxHealth = 100; // Maximum health for the slider
    public int goldReward = 100; // Gold given when the enemy dies
    public int damageFromProjectile = 50; // Damage taken from player projectiles
    public Slider healthBar; // Reference to the health bar slider
    public Slider delayedHealthBar; // Reference to the delayed health bar slider
    public float healthBarUpdateDuration = 1.5f; // Duration for the health bar to update

    private float lastDamageTime; // Time when the last damage was taken
    private bool isUpdatingDelayedHealthBar = false; // Flag to check if the delayed health bar coroutine is running

    // Attack and movement variables
    public Animator animator; // Ensure to assign this in the Unity Editor
    public Transform player;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;
    private bool playerInRange = false;

    private bool isDead = false;

    private void Start()
    {
        // Initialize health
        health = maxHealth;

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

    private void Update()
    {
        if (isDead) return;

        if (playerInRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void AttackPlayer()
    {
        Vector2 direction = player.position - transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal attack
            animator.SetBool("IsStraight", true);
            animator.SetBool("Is45up", false);
            animator.SetBool("Is45down", false);
        }
        else
        {
            if (direction.y > 0)
            {
                // Upward attack
                animator.SetBool("Is45up", true);
                animator.SetBool("IsStraight", false);
                animator.SetBool("Is45down", false);
            }
            else
            {
                // Downward attack
                animator.SetBool("Is45down", true);
                animator.SetBool("IsStraight", false);
                animator.SetBool("Is45up", false);
            }
        }

        // Truck attack animation
        animator.SetTrigger("TruckAttack");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        lastDamageTime = Time.time;
        animator.SetTrigger("IsTakingDamage");

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

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("IsDead");

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.AddGold(goldReward);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
        else if (collision.CompareTag("PlayerProjectile"))
        {
            TakeDamage(damageFromProjectile);
            Destroy(collision.gameObject); // Destroy the projectile on collision
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
