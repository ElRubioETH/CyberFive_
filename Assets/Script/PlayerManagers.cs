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

    private Rigidbody2D rb;
    private SpriteRenderer bodySpriteRenderer;
    private SpriteRenderer armSpriteRenderer;
    private SpriteRenderer gunSpriteRenderer;
    private bool isGrounded;
    private bool isAttacking;
    private bool isGunMode;
    private bool isDefaultMode;
    private bool canDoubleJump; // Flag to allow double jump
    private bool isDoubleJumping; // Flag to track double jump state
    private int maxHealth = 100;
    private int currentHealth;

    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Reference to the point from which the projectile is fired
    public float bulletForce = 20f; // Editable force for the projectile
    public float firePointDistance = 1f; // Distance of firePoint from the player

    public Sprite[] weaponSprites; // Array to store weapon sprites
    private int currentWeaponIndex = 0; // Track the current weapon index

    public float circleRadius = 1.5f; // Radius of the circle around the player, adjustable

    public int gold = 0; // Player's gold

    // Existing variables
    public GameObject inventoryPanel; // Reference to the inventory panel
    public Text goldText; // Reference to the gold Text element

    void Start()
    {
        // Existing code
        HideAllWeaponButtons(); // Hide all weapon buttons initially
        ShowWeaponButton(0); // Display only the first weapon button initially
        rb = GetComponent<Rigidbody2D>();
        bodySpriteRenderer = GetComponent<SpriteRenderer>();
        armSpriteRenderer = armGameObject.GetComponent<SpriteRenderer>();
        gunSpriteRenderer = gunGameObject.GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthBar();
        UpdateArmAndGunSprites();
        UpdateGoldText(); // Update gold text on start
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

        if (Input.GetKeyDown(KeyCode.G))
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
        if (Input.GetKeyDown(KeyCode.H)) // Key to switch to default mode
        {
            isDefaultMode = !isDefaultMode;
            animator.SetBool("IsDefaultMode", isDefaultMode);
            if (isDefaultMode)
            {
                isGunMode = false;
                animator.SetBool("IsGunMode", false);
                armGameObject.SetActive(false);
                gunGameObject.SetActive(false);
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
            gunSpriteRenderer.sprite = weaponSprites[currentWeaponIndex]; // Use the current weapon sprite
            armGameObject.SetActive(true);
            gunGameObject.SetActive(true);
        }
        else
        {
            armGameObject.SetActive(false);
            gunGameObject.SetActive(false);
        }
    }

    public void ChangeWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weaponSprites.Length)
        {
            currentWeaponIndex = weaponIndex; // Update the current weapon index
            gunSpriteRenderer.sprite = weaponSprites[weaponIndex];
            animator.SetInteger("WeaponIndex", weaponIndex);
            Debug.Log($"Weapon changed to: {weaponIndex}");

            // Equip the weapon
            isGunMode = true;
            animator.SetBool("IsGunMode", isGunMode);
            UpdateArmAndGunSprites();
        }
        else
        {
            Debug.LogError($"Weapon index {weaponIndex} is out of bounds.");
        }
    }

    void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bool isFlipped = mousePosition.x < transform.position.x;

        bodySpriteRenderer.flipX = isFlipped;

        // Flip the arm and gun along the Y-axis if the cursor is to the left
        armSpriteRenderer.flipY = isFlipped;
        gunSpriteRenderer.flipY = isFlipped;

        // Calculate the position of the arm and gun along the circle's radius
        Vector2 circlePosition = direction.normalized * circleRadius;

        if (isFlipped)
        {
            // If flipped, move the arm and gun to the opposite side of the circle
            circlePosition = new Vector2(-circlePosition.x, circlePosition.y);
        }

        armGameObject.transform.localPosition = circlePosition;
        gunGameObject.transform.localPosition = circlePosition;

        // Adjust arm and gun rotation based on the angle
        armGameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
        gunGameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);

        float distance = Mathf.Min(direction.magnitude, circleRadius);
        Vector3 firePointPosition = transform.position + (Vector3)direction.normalized * distance;
        firePoint.position = firePointPosition;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Gold: {gold}");
        UpdateGoldText(); // Update gold text whenever gold amount changes
    }

    void UpdateGoldText()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold;
        }
    }

    public void UpdateInventory(int weaponIndex)
    {
        ShowWeaponButton(weaponIndex);
    }

    public void HideAllWeaponButtons()
    {
        for (int i = 0; i < 10; i++)
        {
            string buttonName = "WeaponButton" + (i + 1);
            Transform buttonTransform = inventoryPanel.transform.Find(buttonName);
            if (buttonTransform != null)
            {
                buttonTransform.gameObject.SetActive(false);
            }
        }
    }

    public void ShowWeaponButton(int weaponIndex)
    {
        string buttonName = "WeaponButton" + (weaponIndex + 1);
        Transform buttonTransform = inventoryPanel.transform.Find(buttonName);
        if (buttonTransform != null)
        {
            buttonTransform.gameObject.SetActive(true);
        }
    }
}
