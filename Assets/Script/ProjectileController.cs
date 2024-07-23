using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 20f; // Speed of the projectile
    private float damage;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the projectile.");
            return;
        }

        // Set the projectile's velocity
        rb.velocity = transform.right * speed; // Assuming projectile is fired along its right direction
    }

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the projectile on collision
        }
    }
}
