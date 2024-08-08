using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlueHead : MonoBehaviour
{
    
    public Transform firepoint;
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthBar;
    private Animator animator;
    private Vector3 originalScale;
    private bool isAttacking = false;
    public int attackDamage = 20;
    private float attackDelay = 1f; // Delay before the player takes damage
    public int goldReward = 100;
    private bool isDead = false;

    public GameObject projectilePrefab; // Reference to the projectile prefab
    public float projectileForce = 10f; // Adjustable force for the projectile
    public float projectileSpeed = 5f; // Adjustable speed for the projectile
    public int projectileDamage = 10; // Damage dealt by the projectile

    public GameObject hitboxPrefab; // Reference to the hitbox prefab
    public float hitboxDuration = 1f; // Duration the hitbox stays active
    public Vector3 hitboxOffset = Vector3.zero; // Offset position for the hitbox

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = 1f; // Set initial health bar to full
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isAttacking && !isDead)
        {
            // Add your attack logic here if needed
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        // Update the health bar gradually
        healthBar.value = currentHealth / maxHealth;
        animator.SetTrigger("TakeDamage");
        ShootProjectile(); // Call the shoot method when taking damage
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null || firepoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (FindObjectOfType<PlayerController>().transform.position - transform.position).normalized;
            rb.velocity = direction * projectileSpeed;
            rb.AddForce(direction * projectileForce, ForceMode2D.Impulse);
        }

        BHProjectile projectileScript = projectile.GetComponent<BHProjectile>();
        if (projectileScript != null)
        {
            projectileScript.damage = projectileDamage;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        StopAllCoroutines();
        PlayerController playerController = FindObjectOfType<PlayerController>();
        animator.SetTrigger("Dead");
        if (playerController != null)
        {
            playerController.AddGold(goldReward);
        }
        Destroy(gameObject, 5f);
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }

        animator.ResetTrigger("Attack");
        animator.ResetTrigger("TakeDamage");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isAttacking && !isDead)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            StartCoroutine(DealDamageWithDelay());
            StartCoroutine(RepeatedAttack());
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = false;
            StopCoroutine(RepeatedAttack());
        }
    }
    IEnumerator RepeatedAttack()
    {
        while (isAttacking)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.3f); // Adjust the delay for animation timing
            CreateHitbox();
            yield return new WaitForSeconds(2f - 0.3f); // 2 seconds delay between each attack minus animation time
        }
    }

    IEnumerator DealDamageWithDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        CreateHitbox();
        isAttacking = false;
    }

    void CreateHitbox()
    {
        if (hitboxPrefab == null) return;

        Vector3 spawnPosition = transform.position + hitboxOffset;
        GameObject hitbox = Instantiate(hitboxPrefab, spawnPosition, Quaternion.identity);
        Hitbox hitboxScript = hitbox.GetComponent<Hitbox>();
        if (hitboxScript != null)
        {
            hitboxScript.SetDamage(attackDamage);
        }
        Destroy(hitbox, hitboxDuration); // Destroy hitbox after duration
    }


}
