using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float startPointX = -5f;
    public float endPointX = 5f;
    private bool movingRight = true;
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthBar;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;
    public int attackDamage = 20;
    private float attackDelay = 1f; // Delay before the player takes damage
    public int goldReward = 100;
    public float healthBarUpdateSpeed = 1f; // Speed at which the health bar updates

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = 1f; // Set initial health bar to full
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            Move();
        }
    }

    void Move()
    {
        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            if (transform.position.x >= endPointX)
            {
                movingRight = false;
                FlipSprite();
            }
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
            if (transform.position.x <= startPointX)
            {
                movingRight = true;
                FlipSprite();
            }
        }
    }

    void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        // Update the health bar gradually
        StartCoroutine(UpdateHealthBarCoroutine(currentHealth / maxHealth));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator UpdateHealthBarCoroutine(float targetValue)
    {
        float startValue = healthBar.value;
        float elapsedTime = 0f;

        while (elapsedTime < healthBarUpdateSpeed)
        {
            healthBar.value = Mathf.Lerp(startValue, targetValue, elapsedTime / healthBarUpdateSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        healthBar.value = targetValue; // Ensure it ends exactly at the target value
    }

    void Die()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.AddGold(goldReward);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            StartCoroutine(DealDamageWithDelay(other.gameObject));
        }
    }

    IEnumerator DealDamageWithDelay(GameObject player)
    {
        yield return new WaitForSeconds(attackDelay);
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage);
        }
        isAttacking = false;
    }
}
