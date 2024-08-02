using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] float pushplayer = 10f;
    Animator animator;
    [SerializeField]
    private GameObject dog;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("trap", true);
    }
    void Update()
    {
        if(dog == null)
        {
            animator.SetBool("trap", false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 pushDirection;
                if (other.transform.position.x < transform.position.x)
                {
                    Debug.Log("nhan vat dang bi day ben trai");
                    // Đẩy sang trái
                    pushDirection = Vector2.left;
                }
                else
                {
                    Debug.Log("nhan vat dang bi day ben phai");
                    // Đẩy sang phải
                    pushDirection = Vector2.right;
                }

                // Áp dụng lực đẩy
                rb.AddForce(pushDirection * pushplayer, ForceMode2D.Impulse);
            }

        }
    }
}
