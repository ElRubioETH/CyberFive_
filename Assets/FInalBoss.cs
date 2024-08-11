using UnityEngine;

public class BossBiker : MonoBehaviour
{
    public Transform player;
    public float meleeRange = 4f;
    public float speed = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;  // Cooldown between melee attacks
    public float gunPhaseHealthThreshold = 50f;
    public float gunPhaseFireRate = 0.8f;
    public Vector2 teleportPosition;
    public Vector2 pointA;
    public Vector2 pointB;
    public GameObject gun;
    public Transform firepoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileDamage = 20f;
    public float circleRadiusSpeed = 100f;
    public float gunSpinSpeed = 50f;

    private float currentHealth = 100f;
    private bool isGunPhase = false;
    private bool isMovingToA = true;
    private float nextFireTime = 0f;
    private float nextAttackTime = 0f;  // Timer for melee attack cooldown
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 originalScale;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isGunPhase)
        {
            HandleMeleePhase();
        }
        else
        {
            HandleGunPhase();
        }

        if (currentHealth <= gunPhaseHealthThreshold && !isGunPhase)
        {
            StartGunPhase();
        }
    }

    void HandleMeleePhase()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= meleeRange)
        {
            // Stop moving and attack, with cooldown
            if (Time.time >= nextAttackTime)
            {
                rb.velocity = Vector2.zero;
                animator.Play("BossAttack");
                player.GetComponent<PlayerController>().TakeDamage(Mathf.RoundToInt(attackDamage));
                nextAttackTime = Time.time + attackCooldown;  // Set the next attack time
            }
        }
        else
        {
            // Move towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
            animator.Play("BossMove");
        }
    }

    void StartGunPhase()
    {
        isGunPhase = true;
        rb.velocity = Vector2.zero;
        animator.Play("BossGunPhaseIdle");

        // Teleport to the specified position
        transform.position = teleportPosition;

        // Attach gun to the spinning circle
        gun.transform.SetParent(transform);
    }

    void HandleGunPhase()
    {
        // Move between points A and B
        if (isMovingToA)
        {
            MoveToPoint(pointA);
        }
        else
        {
            MoveToPoint(pointB);
        }

        // Flip the boss
        if ((isMovingToA && transform.position.x >= pointA.x) ||
            (!isMovingToA && transform.position.x <= pointB.x))
        {
            isMovingToA = !isMovingToA;
            Flip();
        }

        // Spin the gun and aim at the player
        gun.transform.RotateAround(transform.position, Vector3.forward, gunSpinSpeed * Time.deltaTime);

        // Fire at the player with cooldown
        if (Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + gunPhaseFireRate;
        }
    }

    void MoveToPoint(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, firepoint.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        if (projectileRb != null)
        {
            Vector2 direction = (player.position - firepoint.position).normalized;
            projectileRb.velocity = direction * projectileSpeed;

            // Set projectile damage
            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.damage = Mathf.RoundToInt(projectileDamage);
            }
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody2D component.");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
