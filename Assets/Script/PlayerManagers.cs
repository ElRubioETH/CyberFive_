using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using JetBrains.Annotations;
using Unity.VisualScripting;


public class PlayerController : MonoBehaviour
{
    public GameObject[] gunGameObjects; // Array to store the gun GameObjects
    public float[] projectileDamageValues;
    public bool isPlayerInCar = false;
    private bool isTimeStopped = false;
    private bool isTimeStopOnCooldown = false;
    private float previousFixedDeltaTime;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public Slider healthBar;
    private SpriteRenderer gunSpriteRenderer; // Reference to the SpriteRenderer component
    private int currentWeaponIndex = 0;
    public GameObject armGameObject; // Reference to the arm GameObject
    
    public Sprite armSprite; // Sprite for the arm



    private bool isFiring = false;
    public float fireRate = 0.2f; // Adjust this value to change the fire rate

    public Rigidbody2D rb;
    private SpriteRenderer bodySpriteRenderer;
    private SpriteRenderer armSpriteRenderer;
  
    public float ValueX { get; set; }

    private bool isGrounded;
    private bool isAttacking;
    private bool isGunMode;
    private bool isDefaultMode;
    private bool canDoubleJump; // Flag to allow double jump
    private bool isDoubleJumping; // Flag to track double jump state
    private int maxHealth = 100;
    private int currentHealth;
    public List<GameObject> ignoreTimeStopObjects;
    public GameObject car;
    public GameObject r34;
    private CarController carController;
    private R34 r34Controller;
    private Collider2D playerCollider;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Reference to the point from which the projectile is fired
    public float bulletForce = 20f; // Editable force for the projectile
    public float bulletRadius = 1.5f; // New radius for bullet fire point
    public float[] weaponDamageValues; // Array to store damage values for each weapon
    private float currentWeaponDamage; // Variable to store the current weapon's damage
    public float circleRadius = 1.5f; // Radius of the circle around the player, adjustable
    public int gold = 0; // Player's gold
    public GameObject inventoryPanel; // Reference to the inventory panel
    public TMP_Text goldText; // Reference to the gold Text element
    // Climbing variables
    public Transform climbCheck; // Check if the player is near a climbable object
    public LayerMask climbableLayer; // Layer for climbable objects
    private bool isClimbing = false;
    // Shoot effect variables
    public GameObject shootEffectPrefab; // Reference to the shoot effect prefab
    public Transform shootEffectPoint; // Reference to the point where shoot effect will appear
    // Time stop audio
    public AudioSource timeStopAudio; // Reference to the audio source for time stop



    public void EnableControls()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.one;
    }
    public void DisableControls()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
    }


    void Start()
    {
        
        HideAllWeaponButtons();
        ShowWeaponButton(0);
        rb = GetComponent<Rigidbody2D>();
        bodySpriteRenderer = GetComponent<SpriteRenderer>();
        armSpriteRenderer = armGameObject.GetComponent<SpriteRenderer>();
        
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthBar();
        UpdateArmAndGunSprites();

        previousFixedDeltaTime = Time.fixedDeltaTime;
        StartCoroutine(LogGoldAmount());
        UpdateGoldText();
        carController = car.GetComponent<CarController>();
        r34Controller = r34.GetComponent<R34>();
        playerCollider = GetComponent<Collider2D>();
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        // Initialize guns
        for (int i = 0; i < gunGameObjects.Length; i++)
        {
            if (i == currentWeaponIndex)
            {
                gunGameObjects[i].SetActive(true);
            }
            else
            {
                gunGameObjects[i].SetActive(false);
            }
        }
    }

    void Update()

    {

        carController = car.GetComponent<CarController>();
        r34Controller = r34.GetComponent<R34>();

        if (Input.GetKeyDown(KeyCode.C)) // Key to get in/out of the car
        {
            ToggleCar();
        }
        if (Input.GetKeyDown(KeyCode.C)) // Key to get in/out of the car
        {
            ToggleCarR34();
        }

        if (carController.isPlayerInCar)
        {
            // Disable player controls if in the car
            return;
        }
        float moveInput = 0;


        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1;
        }

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (!isAttacking)
        {
            rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

            if (moveInput != 0)
            {
                if (isGunMode)
                {
                    animator.SetBool("IsWalkingGun", !Input.GetKey(KeyCode.LeftShift));
                    animator.SetBool("IsRunningGun", Input.GetKey(KeyCode.LeftShift));
                }
                else
                {
                    animator.SetBool("IsWalking", !Input.GetKey(KeyCode.LeftShift));
                    animator.SetBool("IsRunning", Input.GetKey(KeyCode.LeftShift));
                    bodySpriteRenderer.flipX = moveInput < 0;
                }
            }
            else
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsWalkingGun", false);
                animator.SetBool("IsRunningGun", false);
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
        





        if (Input.GetKeyDown(KeyCode.Q))
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

                armGameObject.transform.localRotation = Quaternion.identity;

                animator.SetInteger("GunDirection", 0);
                foreach (var gun in gunGameObjects)
                {
                    gun.SetActive(false);
                }
            }

        }

        if (isGunMode)
        {
            AimAtMouse();
            if (Input.GetKeyDown(KeyCode.F)) // Key to start firing
            {
                isFiring = true;
                StartCoroutine(AutoFire());
            }
            else if (Input.GetKeyUp(KeyCode.F)) // Key to stop firing
            {
                isFiring = false;
            }
        }


        CheckGrounded();
        CheckClimbable(); // Check if the player can climb
        UpdateAnimator();
        if (Input.GetKeyDown(KeyCode.T) && !isTimeStopOnCooldown)
        {
            ToggleTimeStop();
        }
        
    }




    void ToggleCar()
    {
        if (carController.isPlayerInCar)
        {
            // Player gets out of the car
            carController.ExitCar(transform);
            carController.DetachPlayer(transform); // Detach player from the car
            animator.SetBool("BikerInCar", false);
            Physics2D.IgnoreCollision(car.GetComponent<Collider2D>(), playerCollider, false); // Re-enable collision

            // Change virtual camera targets back to the player
            virtualCamera.LookAt = transform;
            virtualCamera.Follow = transform;
        }
        else if (carController.IsPlayerNearCar())
        {
            // Player gets in the car
            transform.position = car.transform.position; // Align player with car position
            carController.EnterCar();
            carController.AttachPlayer(transform); // Attach player to the car
            animator.SetBool("BikerInCar", true);
            Physics2D.IgnoreCollision(car.GetComponent<Collider2D>(), playerCollider, true); // Ignore collision

            // Change virtual camera targets to the car
            virtualCamera.LookAt = car.transform;
            virtualCamera.Follow = car.transform;

        }
    }
    void ToggleCarR34()
    {
        if (r34Controller.isPlayerInCar)
        {
            // Player gets out of the car
            r34Controller.ExitCar(transform);
            r34Controller.DetachPlayer(transform); // Detach player from the car
            animator.SetBool("BikerInCar", false);
            Physics2D.IgnoreCollision(r34.GetComponent<Collider2D>(), playerCollider, false); // Re-enable collision

            // Change virtual camera targets back to the player
            virtualCamera.LookAt = transform;
            virtualCamera.Follow = transform;
        }
        else if (r34Controller.IsPlayerNearCar())
        {
            // Player gets in the car
            transform.position = r34.transform.position; // Align player with car position
            r34Controller.EnterCar();
            r34Controller.AttachPlayer(transform); // Attach player to the car
            animator.SetBool("BikerInCar", true);
            Physics2D.IgnoreCollision(car.GetComponent<Collider2D>(), playerCollider, true); // Ignore collision

            // Change virtual camera targets to the car
            virtualCamera.LookAt = r34.transform;
            virtualCamera.Follow = r34.transform;

        }
    }




    bool IsIgnoredByTimeStop(GameObject obj)
    {
        return ignoreTimeStopObjects.Contains(obj);
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
    void ToggleTimeStop()
    {
        isTimeStopped = !isTimeStopped;

        if (isTimeStopped)
        {
            Time.timeScale = 0f; // Stop time
            Time.fixedDeltaTime = 0f; // Stop physics updates
            PauseAllAnimations();
            if (timeStopAudio != null)
            {
                timeStopAudio.Play(); // Play time stop audio
            }

        }
        else
        {
            Time.timeScale = 1f; // Resume time
            Time.fixedDeltaTime = previousFixedDeltaTime; // Resume physics updates
            ResumeAllAnimations();
            StartCoroutine(TimeStopCooldown()); // Start cooldown after disabling time stop
        }
    }
    void PauseAllAnimations()
    {
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (Animator anim in animators)
        {
            if (anim.gameObject != gameObject && anim.gameObject != shootEffectPrefab && !ignoreTimeStopObjects.Contains(anim.gameObject)) // Skip the player, shoot effect, and ignored objects
            {
                anim.enabled = false;
            }
        }

        Rigidbody2D[] rigidbodies = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in rigidbodies)
        {
            if (rb.gameObject != gameObject && !ignoreTimeStopObjects.Contains(rb.gameObject)) // Skip the player and ignored objects
            {
                rb.simulated = false;
            }
        }
    }



    void ResumeAllAnimations()
    {
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (Animator anim in animators)
        {
            if (anim.gameObject != gameObject && anim.gameObject != shootEffectPrefab && !ignoreTimeStopObjects.Contains(anim.gameObject)) // Skip the player, shoot effect, and ignored objects
            {
                anim.enabled = true;
            }
        }

        Rigidbody2D[] rigidbodies = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in rigidbodies)
        {
            if (rb.gameObject != gameObject && !ignoreTimeStopObjects.Contains(rb.gameObject)) // Skip the player and ignored objects
            {
                rb.simulated = true;
            }
        }
    }


    void CheckClimbable()
    {
        Collider2D climbable = Physics2D.OverlapCircle(climbCheck.position, 0.2f, climbableLayer);
        if (climbable && Input.GetKey(KeyCode.W))
        {
            StartClimbing();
        }
        else if (!climbable || Input.GetKeyUp(KeyCode.W))
        {
            StopClimbing();
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
        rb.velocity = new Vector2(rb.velocity.x, walkSpeed);
        animator.SetBool("IsClimbing", true);
    }

    void StopClimbing()
    {
        isClimbing = false;
        animator.SetBool("IsClimbing", false);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
    }
    private IEnumerator AutoFire()
    {
        while (isFiring)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }


    void UpdateAnimator()
    {
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsGunMode", isGunMode);
        animator.SetBool("IsDoubleJumping", isDoubleJumping); // Add this line
        animator.SetBool("IsClimbing", isClimbing); // Add this line
        animator.SetBool("IsFalling", !isGrounded && rb.velocity.y < 0); // Add this line

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
        animator.SetTrigger("TakeDamage");
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

    public void Die()
    {
        isDefaultMode = !isDefaultMode;
        animator.SetBool("IsDefaultMode", isDefaultMode);
        if (isDefaultMode)
        {
            isGunMode = false;
            animator.SetBool("IsGunMode", false);
            armGameObject.SetActive(false);

            armGameObject.transform.localRotation = Quaternion.identity;

            animator.SetInteger("GunDirection", 0);
            foreach (var gun in gunGameObjects)
            {
                gun.SetActive(false);
            }
        }
        animator.SetTrigger("Die");
        rb.velocity = Vector2.zero; // Stop player movement
        this.enabled = false; // Disable the script
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) // Added this block
        {
            collider.enabled = false; // Added this block
        }
        
        if (rb != null) // Added this block
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; // Added this block
        }
        
    }

    void Shoot()
    {
        if (firePoint != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

            if (projectileController != null)
            {
                projectileController.SetDamage(projectileDamageValues[currentWeaponIndex]);
            }

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - firePoint.position).normalized;
            rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
            animator.SetTrigger("Shoot");

            // Create shoot effect
            if (shootEffectPrefab != null)
            {
                GameObject shootEffect = Instantiate(shootEffectPrefab, shootEffectPoint.position, shootEffectPoint.rotation);
                shootEffect.transform.parent = shootEffectPoint;
                Destroy(shootEffect, 0.5f); // Destroy the effect after 0.5 seconds
            }

            // Destroy the projectile after 1 second
            Destroy(projectile, 1f);
        }
    }


    void UpdateArmAndGunSprites()
    {
        if (isGunMode)
        {
            armSpriteRenderer.sprite = armSprite;

            armGameObject.SetActive(true);

        }
        else
        {
            armGameObject.SetActive(false);

        }
    }



    public void ChangeWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < gunGameObjects.Length)
        {
            // Deactivate current gun
            if (gunGameObjects[currentWeaponIndex] != null)
            {
                gunGameObjects[currentWeaponIndex].SetActive(false);
            }

            // Update the current weapon index
            currentWeaponIndex = weaponIndex;

            // Activate new gun
            if (gunGameObjects[currentWeaponIndex] != null)
            {
                gunGameObjects[currentWeaponIndex].SetActive(true);
                Debug.Log($"Weapon changed to index: {weaponIndex}, Gun GameObject: {gunGameObjects[weaponIndex].name}");
            }
            else
            {
                Debug.LogError($"Gun GameObject at index {weaponIndex} is null.");
            }

            // Equip the weapon
            isGunMode = true;
            animator.SetBool("IsGunMode", isGunMode);
        }
        else
        {
            Debug.LogError($"Weapon index {weaponIndex} is out of bounds.");
        }
    }




    public void BuyWeapon(int weaponIndex, int weaponCost)
    {
        Debug.Log("Attempting to buy weapon...");
        Debug.Log($"Current Gold: {gold}, Weapon Cost: {weaponCost}");

        if (gold >= weaponCost)
        {
            // Deduct the cost of the weapon from the gold
            gold -= weaponCost;
            UpdateGoldText();
            Debug.Log($"Gold after deduction: {gold}");

            // Update the gold text to reflect the new amount
            Debug.Log("Gold text updated.");

            // Change the weapon to the selected one
            ChangeWeapon(weaponIndex);
        }
        else
        {
            Debug.Log("Not enough gold to buy this weapon.");
        }
    }

    public void OnWeaponButtonClicked(int weaponIndex)
    {
        ChangeWeapon(weaponIndex);
    }

    private IEnumerator LogGoldAmount()
    {
        while (true)
        {
            Debug.Log($"Current gold: {gold}");
            yield return new WaitForSeconds(2f);
        }
    }










    void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bool isFlipped = mousePosition.x < transform.position.x;

        bodySpriteRenderer.flipX = isFlipped;

        // Flip the arm along the Y-axis if the cursor is to the left
        armSpriteRenderer.flipY = isFlipped;

        // Calculate the position of the arm along the circle's radius
        Vector2 circlePosition = direction.normalized * circleRadius;

        if (isFlipped)
        {
            // If flipped, move the arm to the opposite side of the circle
            circlePosition = new Vector2(-circlePosition.x, circlePosition.y);
        }

        armGameObject.transform.localPosition = circlePosition;

        // Ensure the gun follows the arm
        foreach (var gun in gunGameObjects)
        {
            if (gun.activeSelf)
            {
                SpriteRenderer gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
                gunSpriteRenderer.flipY = isFlipped;
                gun.transform.localPosition = circlePosition;
                gun.transform.localRotation = Quaternion.Euler(0, 0, angle);

            }
        }

        // Adjust fire point and shoot effect point based on the bullet radius
        float distance = Mathf.Min(direction.magnitude, bulletRadius);
        Vector3 firePointPosition = transform.position + (Vector3)direction.normalized * distance;
        firePoint.position = firePointPosition;
        shootEffectPoint.position = firePointPosition;

        // Adjust the rotation of the fire point and shoot effect point
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
        shootEffectPoint.rotation = Quaternion.Euler(0, 0, angle);
        armGameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // Flip the shoot effect point based on isFlipped
        shootEffectPoint.localScale = new Vector3(isFlipped ? -1 : 1, 1, 1);
    }



    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldText(); // Update the gold text whenever gold amount changes
        Debug.Log($"Gold added: {amount}. Current gold: {gold}");
    }

    public void UpdateGoldText()
    {
        if (goldText != null)
        {
            goldText.text = $" {gold}";
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



    private IEnumerator TimeStopCooldown()
    {
        isTimeStopOnCooldown = true;
        yield return new WaitForSeconds(5f); // 5 second cooldown
        isTimeStopOnCooldown = false;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= 20;
            animator.SetTrigger("TakeDamage");
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                // Handle player death  
            }
            UpdateHealthBar();
        }
        void UpdateHealthBar()
        {
            healthBar.value = (float)currentHealth / maxHealth;

        }

    }


}