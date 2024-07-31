using UnityEngine;

public class R34 : MonoBehaviour
{
    public bool isPlayerInCar = false;
    public float flySpeed = 5f;
    public float flyUpSpeed = 3f;
    public float flyDownSpeed = 3f;
    public float nitroSpeedMultiplier = 2f; // Multiplier for nitro speed
    public Animator carAnimator;
    private Rigidbody2D rb;
    private bool isPlayerNearCar = false;
    public AudioSource carAudioSource;
    private PlayerController playerController;
    public GameObject player;
    public GameObject[] gunGameObjects;
    public GameObject nitroEffect; // Nitro effect GameObject

    public AudioSource sceneAudioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isPlayerInCar)
        {
            HandleCarMovement();
        }
    }

    public void EnterCar()
    {
        isPlayerInCar = true;
        carAudioSource.Play();
        sceneAudioSource.Pause();

        playerController = player.GetComponent<PlayerController>();
        foreach (var gun in gunGameObjects)
        {
            gun.SetActive(false);
        }
    }

    public void ExitCar(Transform playerTransform)
    {
        isPlayerInCar = false;
        playerTransform.position = transform.position; // Align player with car position
        carAudioSource.Stop();
        sceneAudioSource.UnPause();
        playerController = player.GetComponent<PlayerController>();
    }

    void HandleCarMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = 0;

        if (Input.GetKey(KeyCode.W))
        {
            moveVertical = flyUpSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVertical = -flyDownSpeed;
        }

        // Check for nitro boost
        bool isNitroActive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentFlySpeed = isNitroActive ? flySpeed * nitroSpeedMultiplier : flySpeed;
        Vector2 movement = new Vector2(moveHorizontal * currentFlySpeed, moveVertical);
        rb.velocity = movement;

        // Activate or deactivate nitro effect
        if (nitroEffect != null)
        {
            nitroEffect.SetActive(isNitroActive);
        }

        UpdateAnimation(moveHorizontal > 0, moveHorizontal < 0, moveHorizontal == 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipCarSprite();
        }
    }

    public void AttachPlayer(Transform playerTransform)
    {
        playerTransform.parent = transform; // Attach player to the car
    }

    public void DetachPlayer(Transform playerTransform)
    {
        playerTransform.parent = null; // Detach player from the car
    }

    void FlipCarSprite()
    {
        Vector3 carScale = transform.localScale;
        carScale.x *= -1;
        transform.localScale = carScale;
    }

    void UpdateAnimation(bool isForward, bool isBackward, bool isIdle)
    {
        if (isForward)
        {
            Vector3 carScale = transform.localScale;

            if (carScale.x < 0)
            {
                carAnimator.SetBool("isForward", false);
                carAnimator.SetBool("isBackward", false);
            }
            else
            {
                carAnimator.SetBool("isForward", true);
                carAnimator.SetBool("isBackward", false);
                carAnimator.SetBool("isIdle", false);
            }
        }
        else if (isBackward)
        {
            Vector3 carScale = transform.localScale;

            if (carScale.x < 0)
            {
                carAnimator.SetBool("isForward", false);
                carAnimator.SetBool("isBackward", false);
            }
            else
            {
                carAnimator.SetBool("isForward", false);
                carAnimator.SetBool("isBackward", true);
                carAnimator.SetBool("isIdle", false);
            }
        }
        else if (isIdle)
        {
            carAnimator.SetBool("isForward", false);
            carAnimator.SetBool("isBackward", false);
            carAnimator.SetBool("isIdle", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearCar = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearCar = false;
        }
    }

    public bool IsPlayerNearCar()
    {
        return isPlayerNearCar;
    }
}
