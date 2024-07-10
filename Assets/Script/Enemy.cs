using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int goldReward = 100; // Gold given when the enemy dies
    public int damageFromProjectile = 50; // Damage taken from player projectiles

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
        Destroy(gameObject);
    }
}
