using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BiueHead : MonoBehaviour
{
    // Health and damage variables
    public int health = 100;
    public int maxHealth = 100;
    public int goldReward = 100;
    public int damageFromProjectile = 50;
    public Slider healthBar;
    public Slider delayedHealthBar;
    public float healthBarUpdateDuration = 1.5f;

    private float lastDamageTime;
    private bool isUpdatingDelayedHealthBar = false;

    // Attack and movement variables
    public Animator animator;
    public Transform player;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;
    private bool playerInRange = false;

    private bool isDead = false;

    // Player health management
    public int playerHealth = 100; // Player's initial health
    public int maxPlayerHealth = 100; // Player's maximum health
    public int playerDamage = 20; // Damage dealt to the player by the truck
    public Slider playerHealthBar;

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

        // Initialize player health bar
        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = maxPlayerHealth;
            playerHealthBar.value = playerHealth;
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
        animator.SetTrigger("TruckAttack");

        // Damage player
        playerHealth -= playerDamage;
        if (playerHealthBar != null)
        {
            playerHealthBar.value = playerHealth;
        }

        if (playerHealth <= 0)
        {
            // Player dies (implement player death logic here)
            Debug.Log("Player died!");
        }
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
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateHealthBar(delayedHealthBar, delayedHealthBar.value, health, healthBarUpdateDuration));
        isUpdatingDelayedHealthBar = false;
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("IsDead");
        // Reward player with gold (implement reward logic here)
        // Destroy enemy truck or disable it
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Stop the truck movement
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Resume truck movement (implement movement logic here if needed)
        }
    }
}
