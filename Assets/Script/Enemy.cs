using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100; // Maximum health for the slider
    public int goldReward = 100; // Gold given when the enemy dies
    public int damageFromProjectile = 50; // Damage taken from player projectiles
    public Slider healthBar; // Reference to the health bar slider
    public Slider delayedHealthBar; // Reference to the delayed health bar slider
    public float healthBarUpdateDuration = 1.5f; // Duration for the health bar to update

    private float lastDamageTime; // Time when the last damage was taken
    private bool isUpdatingDelayedHealthBar = false; // Flag to check if the delayed health bar coroutine is running

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
