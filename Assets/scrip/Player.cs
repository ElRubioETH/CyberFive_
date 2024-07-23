using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumping = 10f;
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float runspeed = 10f;
    [SerializeField] private TextMeshProUGUI textmoney;
    [SerializeField] private TextMeshProUGUI missionmoney;
    [SerializeField] private Slider hp;
    [SerializeField] private GameObject gameover;
    private float health = 100f;
    private int money = 0;
    private float horizontal;
    private bool Rightmoving = true;
    private Rigidbody2D rb;
    public Transform ground_check;    
    public LayerMask ground_layer;
    private bool IsGrounded;
    private Animator animator;
    private SpriteRenderer sprite;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        hp.value = health;
    }
    // Update is called once per frame
    void Update()
    {  
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded)
            {
                Jump();
            }
        }
       
        Check_Grounded();
        if(health == 0)
        {
            Time.timeScale = 0;
            gameover.SetActive(true);
        }
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumping);
    }
    void Check_Grounded()
    {
        IsGrounded = Physics2D.OverlapCircle(ground_check.position, 0.2f, ground_layer);    
    }
    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * movespeed, rb.velocity.y);
        sprite.flipX = rb.velocity.x < 0f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontal * runspeed, rb.velocity.y);
        }
        /*if (Rightmoving && horizontal < 0f || !Rightmoving && horizontal > 0f)
        {
            Rightmoving = !Rightmoving;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }*/
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("money"))
        {
            Destroy(other.gameObject);
            money++;
            textmoney.text = "x " + money.ToString();
        }
        if (other.gameObject.CompareTag("enemies"))
        {
            health -= 20;
            hp.value = health;
        }
    }
}