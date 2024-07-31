using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireflyKiller : MonoBehaviour
{
    public float moveSpeed = 2f;
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
    public int projectileDamage = 10;
    private bool isDead = false;
    private bool isFlipped = false;
    private bool isFlying = false;
    private Transform playerTransform;
    private bool playerInZone = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = 1f;
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isAttacking && !isDead && playerInZone)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        if (isDead || playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > 1.5f) // Maintain a distance from the player
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            if ((direction.x > 0 && !isFlipped) || (direction.x < 0 && isFlipped))
            {
                FlipSprite();
            }
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    void FlipSprite()
    {
        if (isDead) return;

        Vector3 newScale = originalScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        originalScale = newScale;
        isFlipped = !isFlipped;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthBar.value = currentHealth / maxHealth;

        if (!isDead)
        {
            animator.SetTrigger("TakeDamage");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth <= maxHealth / 2 && !isFlying)
        {
            StartFlying();
        }
    }

    void StartFlying()
    {
        isFlying = true;
        animator.SetBool("Flying", true);
        StartCoroutine(WaitForFlyingAnimation());
    }

    IEnumerator WaitForFlyingAnimation()
    {
        yield return new WaitForSeconds(2f); // Adjust the delay to match the flying animation duration
        if (!isDead)
        {
            StartCoroutine(FlyingAttackRoutine());
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

        animator.SetTrigger("Dead");
        animator.SetBool("Walking", false);
        animator.SetBool("Flying", false);

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

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            playerTransform = other.transform;
            playerInZone = true;
            isAttacking = true;
            StartCoroutine(StartAttackingAfterDelay(other.transform, 1.5f));
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isAttacking && !isDead)
        {
            playerTransform = other.transform;
            playerInZone = true;
            isAttacking = true;
            StartCoroutine(StartAttackingAfterDelay(other.transform, 1.5f));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            playerInZone = false;
            isAttacking = false;
            animator.SetBool("Walking", false);
            animator.SetBool("Flying", false);
            StopAllCoroutines();
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
            if (isFlying)
            {
                ShootProjectile(playerTransform.position);
                yield return new WaitForSeconds(attackDelay);
            }
            else if (Vector2.Distance(transform.position, playerTransform.position) < 2f)
            {
                animator.SetTrigger("Attack1");
                yield return new WaitForSeconds(0.4f);
                // Melee attack logic here (e.g., damage player directly)
            }
            else
            {
                animator.SetTrigger("Attack2");
                yield return new WaitForSeconds(0.4f);
                ShootProjectile(playerTransform.position);
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }

    IEnumerator FlyingAttackRoutine()
    {
        while (isFlying && !isDead)
        {
            ShootProjectile(FindObjectOfType<PlayerController>().transform.position);
            yield return new WaitForSeconds(attackDelay);
        }
    }

    void ShootProjectile(Vector3 targetPosition)
    {
        if (isDead) return;

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
            projectileScript.damage = projectileDamage;
            projectileScript.SetFlip(isFlipped);
        }

        Destroy(projectile, 2f);
    }
}
