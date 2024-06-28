using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public Slider healthBar;

    public GameObject armGameObject; // Reference to the arm GameObject
    public GameObject gunGameObject; // Reference to the gun GameObject
    public Sprite armSprite; // Sprite for the arm
    public Sprite gunSprite; // Sprite for the gun

    private Rigidbody2D rb;
    private SpriteRenderer bodySpriteRenderer;
    private SpriteRenderer armSpriteRenderer;
    private SpriteRenderer gunSpriteRenderer;
    private bool isGrounded;
    private bool isAttacking;
    private bool isGunMode;
    private bool canDoubleJump; // Flag to allow double jump
    private bool isDoubleJumping; // Flag to track double jump state
    private int maxHealth = 100;
    private int currentHealth;

    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Reference to the point from which the projectile is fired
    public float bulletForce = 20f; // Editable force for the projectile
    public float firePointDistance = 1f; // Distance of firePoint from the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodySpriteRenderer = GetComponent<SpriteRenderer>();
        armSpriteRenderer = armGameObject.GetComponent<SpriteRenderer>();
        gunSpriteRenderer = gunGameObject.GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthBar();
        UpdateArmAndGunSprites();
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
                if (!isGunMode)
                {
                    bodySpriteRenderer.flipX = moveInput < 0;
                }
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    Jump();
                }
                else if (canDoubleJump && !isDoubleJumping)
                {
                    DoubleJump();
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.G)) // Key to equip/unequip gun
        {
            isGunMode = !isGunMode;
            animator.SetBool("IsGunMode", isGunMode);
            UpdateArmAndGunSprites();
            if (!isGunMode)
            {
                armGameObject.transform.localRotation = Quaternion.identity;
                gunGameObject.transform.localRotation = Quaternion.identity;
                animator.SetInteger("GunDirection", 0);
            }
        }

        if (isGunMode)
        {
            AimAtMouse();
            if (Input.GetKeyDown(KeyCode.F)) // Key to shoot
            {
                Shoot();
            }
        }

        CheckGrounded();
        UpdateAnimator();
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isGrounded)
        {
            canDoubleJump = true; // Reset double jump ability when grounded
            isDoubleJumping = false;
        }
    }
    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
    }
    void UpdateAnimator()
    {
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsGunMode", isGunMode);
        animator.SetBool("IsDoubleJumping", isDoubleJumping); // Add this line

        if (isGunMode)
        {
            animator.SetBool("IsRunningGun", rb.velocity.x != 0);
            animator.SetBool("IsIdleGun", rb.velocity.x == 0);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetBool("IsJumping", true);
        animator.SetBool("IsDoubleJumping", false); // Reset double jump state
        canDoubleJump = true; // Allow double jump after this jump
    }

    void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetBool("IsJumping", true);
        animator.SetBool("IsDoubleJumping", true); // Set double jump state
        canDoubleJump = false; // Prevent further double jumps until grounded again
        isDoubleJumping = true;
        animator.SetTrigger("DoubleJump"); // Trigger double jump animation
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
        animator.SetTrigger("Die");
    }

    void Shoot()
    {
        if (firePoint != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - firePoint.position).normalized;
            rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
            animator.SetTrigger("Shoot");

            // Destroy the projectile after 1 second
            Destroy(projectile, 1f);
        }
    }


    void UpdateArmAndGunSprites()
    {
        if (isGunMode)
        {
            armSpriteRenderer.sprite = armSprite;
            gunSpriteRenderer.sprite = gunSprite;
            armGameObject.SetActive(true);
            gunGameObject.SetActive(true);
        }
        else
        {
            armGameObject.SetActive(false);
            gunGameObject.SetActive(false);
        }
    }

    public float circleRadius = 1.5f; // Radius of the circle around the player, adjustable

    void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Flip the player based on the mouse position
        if ((mousePosition.x < transform.position.x && !bodySpriteRenderer.flipX) || (mousePosition.x > transform.position.x && bodySpriteRenderer.flipX))
        {
            Flip();
        }

        // Calculate arm and gun positions in a circle around the player
        float armOffsetAngle = angle + 90f; // Offset angle for arm position
        float gunOffsetAngle = angle - 90f; // Offset angle for gun position

        Vector3 armPosition = new Vector3(Mathf.Cos(armOffsetAngle * Mathf.Deg2Rad), Mathf.Sin(armOffsetAngle * Mathf.Deg2Rad), 0f) * circleRadius;
        Vector3 gunPosition = new Vector3(Mathf.Cos(gunOffsetAngle * Mathf.Deg2Rad), Mathf.Sin(gunOffsetAngle * Mathf.Deg2Rad), 0f) * circleRadius;

        armGameObject.transform.position = transform.position + armPosition;
        gunGameObject.transform.position = transform.position + gunPosition;

        armGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        gunGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Update animator parameters based on the angle
        if (angle >= -45 && angle <= 45)
        {
            animator.SetInteger("GunDirection", 0); // Right
        }
        else if (angle > 45 && angle <= 135)
        {
            animator.SetInteger("GunDirection", 1); // Up
        }
        else if (angle > 135 || angle < -135)
        {
            animator.SetInteger("GunDirection", 2); // Left
        }
        else if (angle < -45 && angle >= -135)
        {
            animator.SetInteger("GunDirection", 3); // Down
        }

        // Update firepoint position based on aim direction
        Vector3 firePointOffset = aimDirection * firePointDistance;
        firePoint.position = transform.position + firePointOffset;
    }

    void Flip()
    {
        bodySpriteRenderer.flipX = !bodySpriteRenderer.flipX;

        // Flip Y for arm and gun
        armSpriteRenderer.flipY = !armSpriteRenderer.flipY;
        gunSpriteRenderer.flipY = !gunSpriteRenderer.flipY;

        Vector3 armPosition = armGameObject.transform.localPosition;
        armPosition.x = -armPosition.x;
        armGameObject.transform.localPosition = armPosition;

        Vector3 gunPosition = gunGameObject.transform.localPosition;
        gunPosition.x = -gunPosition.x;
        gunGameObject.transform.localPosition = gunPosition;

        Vector3 firePointPosition = firePoint.localPosition;
        firePointPosition.x = -firePointPosition.x;
        firePoint.localPosition = firePointPosition;

        // Ensure firePoint rotates with the arm and flips Y-axis
        firePoint.localRotation = Quaternion.Euler(0, bodySpriteRenderer.flipX ? 180 : 0, 0);
    }
}
