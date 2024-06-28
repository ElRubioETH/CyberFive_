using UnityEngine;
using UnityEngine.UI; // Required for using UI elements
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public Slider healthBar; // Reference to the UI Slider for the health bar

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isAttacking;
    private int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        float moveInput = 0;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1;
        }

        if (!isAttacking)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            if (moveInput != 0)
            {
                animator.SetBool("IsRunning", true);
                spriteRenderer.flipX = moveInput < 0;
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("IsJumping", true);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Attack());
        }

        CheckGrounded();
        UpdateAnimator();
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void UpdateAnimator()
    {
        animator.SetBool("IsJumping", !isGrounded);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.5f); // Adjust based on attack animation length
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        // Handle player death (e.g., play animation, reload level, etc.)
        animator.SetTrigger("Die");
        // Additional death logic here
    }
}
