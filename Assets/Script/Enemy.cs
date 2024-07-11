using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100; // Maximum health for the slider
    public int goldReward = 100; // Gold given when the enemy dies
    public int damageFromProjectile = 50; // Damage taken from player projectiles
    public GameObject healthBarPrefab; // Prefab of the health bar
    private Slider healthSlider;
    private Transform canvasTransform;

    private void Start()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
        GameObject healthBar = Instantiate(healthBarPrefab, canvasTransform);
        healthSlider = healthBar.GetComponent<Slider>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    private void Update()
    {
        if (healthSlider != null)
        {
            Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
            healthSlider.transform.position = healthBarPosition;
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
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.AddGold(goldReward);
        }
        Destroy(healthSlider.gameObject);
        Destroy(gameObject);
    }
}
