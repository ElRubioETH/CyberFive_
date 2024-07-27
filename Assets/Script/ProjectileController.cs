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
            Spider enemy = other.GetComponent<Spider>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the projectile after hitting an enemy
        }
        if (other.CompareTag("Enemy"))
        {
            Dog enemy = other.GetComponent<Dog>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);

        }
        if (other.CompareTag("Enemy"))
        {
            Truckter enemy = other.GetComponent<Truckter>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);

        }
    }

}
