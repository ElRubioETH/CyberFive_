using UnityEngine;

public class Dump : MonoBehaviour
{
    public Vector2 pointA = new Vector2(-5f, 0f);  // Tọa độ điểm A
    public Vector2 pointB = new Vector2(5f, 0f);   // Tọa độ điểm B
    public float speed = 2.0f;  // Tốc độ di chuyển của vật thể
    public int damage = 20;  // Lượng máu bị trừ khi chạm vào người chơi

    private Vector2 currentTarget;  // Điểm hiện tại mà vật thể sẽ di chuyển tới
    private Rigidbody2D rb;  // Tham chiếu đến Rigidbody2D của vật thể
    private BoxCollider2D boxCollider;  // Tham chiếu đến BoxCollider2D của vật thể
    private SpriteRenderer spriteRenderer;  // Tham chiếu đến SpriteRenderer của vật thể

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
        // Di chuyển vật thể tới điểm hiện tại
        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void CheckSwitchTarget()
    {
        // Nếu vật thể gần điểm hiện tại, chuyển đổi mục tiêu
        if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            spriteRenderer.flipX = !spriteRenderer.flipX;  // Quay đầu lại
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //if (collision.gameObject.CompareTag("Player"))
    //{
    //PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
    //if (playerHealth != null)
    //{
    //playerHealth.TakeDamage(damage);  // Gọi hàm trừ máu của người chơi
    //}
    //}
    //}
}

