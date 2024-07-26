using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float damage;
    public float speed = 20f;
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
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the projectile after hitting an enemy
        }
    }
}
