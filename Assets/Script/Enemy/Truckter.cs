using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Truckter : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float startPointX = -5f;
    public float endPointX = 5f;
    private bool movingRight = true;
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthBar;
    private Animator animator;
    private Vector3 originalScale;
    private bool isAttacking = false;
    public int attackDamage = 20;
    public int goldReward = 100;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;
    public float attackDelay = 2f;
    public int projectileDamage = 10; // Adjustable projectile damage
    private bool isDead = false;
    private bool isFlipped = false;

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
            Move();
        }
    }

    void Move()
    {
        if (isDead) return; // Prevent movement if dead

        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            if (transform.position.x >= endPointX)
            {
                movingRight = false;
                FlipSprite();
            }
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
            if (transform.position.x <= startPointX)
            {
                movingRight = true;
                FlipSprite();
            }
        }

        // Play walk animation
        animator.SetTrigger("Walk");
    }

    void FlipSprite()
    {
        if (isDead) return; // Prevent flipping if dead

        // Flip the scale instead of the sprite renderer flipX
        Vector3 newScale = originalScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        originalScale = newScale; // Update originalScale to the new flipped scale
        isFlipped = !isFlipped; // Update the flip status
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Prevent taking damage if dead

        currentHealth -= damage;
        // Update the health bar immediately
        healthBar.value = currentHealth / maxHealth;

        if (!isDead)
        {
            animator.SetTrigger("TakeDamage");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        StopAllCoroutines();
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.AddGold(goldReward);
        }

        // Play death animation
        animator.SetTrigger("Dead");

        // Stop all other animations
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("TakeDamage");

        // Turn off all colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        // Freeze X and Y position
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            isAttacking = true;
            StopCoroutine("AttackRoutine");
            StartCoroutine(StartAttackingAfterDelay(other.transform, 1.5f));
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isAttacking && !isDead)
        {
            isAttacking = true;
            StopCoroutine("AttackRoutine");
            StartCoroutine(StartAttackingAfterDelay(other.transform, 1.5f));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            isAttacking = false;
            StopAllCoroutines();
            // Play idle animation
            animator.SetTrigger("Idle");
        }
    }

    IEnumerator StartAttackingAfterDelay(Transform playerTransform, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isDead)
        {
            StartCoroutine(AttackRoutine(playerTransform));
        }
    }

    IEnumerator AttackRoutine(Transform playerTransform)
    {
        while (isAttacking && !isDead)
        {
            // Play attack animation
            animator.SetTrigger("Attack2");

            // Wait for the attack animation to finish (1 second)
            yield return new WaitForSeconds(0.4f);

            // Shoot projectile
            ShootProjectile(playerTransform.position);

            // Wait for the attack delay
            yield return new WaitForSeconds(attackDelay);
        }
    }

    void ShootProjectile(Vector3 targetPosition)
    {
        if (isDead) return; // Prevent shooting if dead

        Vector2 direction = (targetPosition - firePoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        }
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.damage = projectileDamage; // Set the damage of the projectile
            projectileScript.SetFlip(isFlipped); // Set the flip status of the projectile
        }

        Destroy(projectile, 2f); // Destroy the projectile after 2 seconds
    }
}
