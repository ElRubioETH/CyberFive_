using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool isPlayerInCar = false;
    public float flySpeed = 5f;
    public float flyUpSpeed = 3f;
    public float flyDownSpeed = 3f;
    public Animator carAnimator;
    private Rigidbody2D rb;
    private bool isPlayerNearCar = false;
    public AudioSource carAudioSource;
    private PlayerController playerController;
    public GameObject player;
    public GameObject[] gunGameObjects;
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

        Vector2 movement = new Vector2(moveHorizontal * flySpeed, moveVertical);
        rb.velocity = movement;

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


        if (carScale.x > 0)
        {
            carAnimator.SetBool("isFlippedForward", true);
            carAnimator.SetBool("isFlippedBackward", false);
        }
        else
        {
            carAnimator.SetBool("isFlippedForward", false);
            carAnimator.SetBool("isFlippedBackward", true);
        }

    }

    void UpdateAnimation(bool isForward, bool isBackward, bool isIdle)
    {
        if (isForward)

        {
            Vector3 carScale = transform.localScale;

            if (carScale.x < 0)
            {
                carAnimator.SetBool("isFlippedForward", true);
                carAnimator.SetBool("isFlippedBackward", false);
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
                carAnimator.SetBool("isFlippedBackward", true);
                carAnimator.SetBool("isFlippedForward", false);
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
            carAnimator.SetBool("isFlippedBackward", false);
            carAnimator.SetBool("isFlippedForward", false);
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
