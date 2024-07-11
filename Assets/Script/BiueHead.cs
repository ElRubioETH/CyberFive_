using UnityEngine;

public class BlueHead : MonoBehaviour
{
    public Vector2 pointA = new Vector2(-5f, 0f);  // Tọa độ điểm A
    public Vector2 pointB = new Vector2(5f, 0f);   // Tọa độ điểm B
    public float speed = 2.0f;  // Tốc độ di chuyển của nhân vật

    private Vector2 currentTarget;  // Điểm hiện tại mà nhân vật sẽ di chuyển tới
    private Rigidbody2D rb;  // Tham chiếu đến Rigidbody2D của nhân vật
    private BoxCollider2D boxCollider;  // Tham chiếu đến BoxCollider2D của nhân vật
    private SpriteRenderer spriteRenderer;  // Tham chiếu đến SpriteRenderer của nhân vật

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTarget = pointB;  // Ban đầu, di chuyển tới điểm B
    }

    void Update()
    {
        MoveToTarget();
        CheckSwitchTarget();
    }

    void MoveToTarget()
    {
        // Di chuyển nhân vật tới điểm hiện tại
        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void CheckSwitchTarget()
    {
        // Nếu nhân vật gần điểm hiện tại, chuyển đổi mục tiêu
        if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            spriteRenderer.flipX = !spriteRenderer.flipX;  // Quay đầu lại
        }
    }
}

