using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumping = 10f;
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private TextMeshProUGUI textmoney;
    private int money = 0;
    private float horizontal;
    private bool Rightmoving = true;
    private Rigidbody2D rb;
    public Transform ground_check;    
    public LayerMask ground_layer;
    private bool IsGrounded;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * movespeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded)
            {
                Jump();
            }
        }
        Flip();
        Check_Grounded();
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumping);
    }
    void Check_Grounded()
    {
        IsGrounded = Physics2D.OverlapCircle(ground_check.position, 0.2f, ground_layer);    
        if(IsGrounded)
        {

        }
    }
    void Flip()
    {
        if (Rightmoving && horizontal < 0f || !Rightmoving && horizontal > 0f)
        {
            Rightmoving = !Rightmoving;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("money"))
        {
            Destroy(other.gameObject);
            money++;
            textmoney.text = "x " +money.ToString(); 
        }
    }
}