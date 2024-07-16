using System.Collections;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    const float groundCheckRadius = 0.01f;
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpPower = 500f;
    [SerializeField] int totalJumps;
    int availableJumps;
    float horizontalValue;
    float runSpeedModifier = 2f;

    [SerializeField] bool isGrounded;
    bool isRunning;
    bool facingRight = true;
    bool multipleJump;
    bool coyoteJump;
    void Awake()
    {
        availableJumps = totalJumps;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Store the horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        //If Lshift is clicked enable isRunning
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        //If Lshift is release disable isRunning
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;

        //If we press Jump button enable jump
        if (Input.GetButtonDown("Jump"))
            Jump();

        //Set the yVelocity in the animator
        anim.SetFloat("yVelocity", rb.velocity.y);

        //If we press F button enable shoot
        if (Input.GetKeyDown(KeyCode.F))
            Shoot();
    }
    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue);

    }
    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        //Check if the GroundCheckObject is colliding with other
        //2D Colliders that are in the "Ground" Layer
        //If yes (isGrounded true) else (isGrounded false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position,groundCheckRadius,groundLayer);
        if(colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJump = false;
            }
        }
        else
        {
            if (wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }

        //As long as we are grounded the "Jump" bool
        //in the animator is disable
        anim.SetBool("Jump", !isGrounded);
    }
    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.1f);
        coyoteJump = false;
    }
    void Jump()
    {
        if (isGrounded)
        {
            multipleJump = true;
            availableJumps--;
            rb.velocity = Vector2.up * jumpPower;
            anim.SetBool("Jump", true);
        }
        else
        {
            if(coyoteJump)
            {
                multipleJump = true;
                availableJumps--;
            }
            if (multipleJump && availableJumps > 0)
            {
                availableJumps--;
                rb.velocity = Vector2.up * jumpPower;
                anim.SetBool("Jump", true);
            }
        }
    }
    void Shoot()
    {
        // Create the bullet at the fire point position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody2D component from the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Apply force to the bullet to make it move
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        anim.SetBool("Shoot", true);
    }
    void Move(float dir)
    {
        #region Move and Run
        //Set value of x using dir and speed
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        //If we are running mulitply with the running modifier
        if (isRunning)
            xVal *= runSpeedModifier;
        //Create Vector2 for the velocity
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        //Set the player's velocity
        rb.velocity = targetVelocity;

        //Store the current scale value
        Vector3 currentScale = transform.localScale;
        //If looking right and clicked left (flip to left)
        if(facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        //If looking left and clicked right (flip to right)
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }

        // 0 idle, 1 run slow, 2 run fast
        //Set the float xVelocity according to the x value
        //of the Rigid2D velocity
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
    }
}
