using UnityEngine;

public class BHProjectile : MonoBehaviour
{
    public int damage = 10;
    public GameObject explosionEffect; // Reference to the explosion effect prefab

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
            TriggerExplosion(); // Trigger the explosion effect
            Destroy(gameObject); // Destroy the projectile
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            TriggerExplosion(); // Trigger the explosion effect
            Destroy(gameObject); // Destroy the projectile
        }
    }

    void TriggerExplosion()
    {
        if (explosionEffect != null)
        {
            // Instantiate the explosion effect at the projectile's position and rotation
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

            // Optionally destroy the explosion effect after its animation is complete
            Animator animator = explosion.GetComponent<Animator>();
            if (animator != null)
            {
                Destroy(explosion, animator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                // If there is no Animator, destroy after a default duration
                Destroy(explosion, 2f); // Adjust this time to match your effect's duration
            }
        }
    }
}
