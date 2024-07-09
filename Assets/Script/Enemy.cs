using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float attackRange = 10f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    public float moveSpeed = 2f;

    public int maxHealth = 100;
    private int currentHealth;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        // Move(); // Comment out or remove this line if not needed

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // void Move()
    // {
    //     // Comment out or remove this function if not needed
    // }

    void AttackPlayer()
    {
        Vector2 direction = player.position - transform.position;

        if (direction.y > 0.5f)
        {
            animator.SetTrigger("PersonShoot45Up");
        }
        else if (direction.y < -0.5f)
        {
            animator.SetTrigger("PersonShoot45Down");
        }
        else
        {
            animator.SetTrigger("PersonShootStraight");
        }

        // Truck attack animation
        animator.SetTrigger("TruckAttack");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("TruckTakeDamage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("TruckDeath");
        // Disable enemy actions, e.g., attacks, movement
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Assume that player projectiles have the tag "PlayerProjectile"
        if (collision.CompareTag("PlayerProjectile"))
        {
            TakeDamage(25);
            Destroy(collision.gameObject); // Destroy the projectile on collision
        }
    }
}
