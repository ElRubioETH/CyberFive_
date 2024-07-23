using UnityEngine;

public class Dump : MonoBehaviour
{
    public float moveDistance = 5.0f; // Khoảng cách di chuyển mỗi lần
    public float boundaryMin = 0.0f;  // Giới hạn thấp của bản đồ
    public float boundaryMax = 10.0f; // Giới hạn cao của bản đồ
    public float speed = 2.0f;        // Tốc độ di chuyển của quái

    private float direction = 1.0f;   // Hướng di chuyển, 1 là tiến tới, -1 là lùi lại

    void Update()
    {
        // Tính toán vị trí mới của quái
        transform.position += Vector3.right * speed * direction * Time.deltaTime;

        // Kiểm tra nếu quái đã vượt qua giới hạn của bản đồ
        if (transform.position.x >= boundaryMax)
        {
            transform.position = new Vector3(boundaryMax, transform.position.y, transform.position.z);
            direction = -1.0f; // Quay đầu lại
        }
        else if (transform.position.x <= boundaryMin)
        {
            transform.position = new Vector3(boundaryMin, transform.position.y, transform.position.z);
            direction = 1.0f; // Quay đầu lại
        }
    }
}
