using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueHead: MonoBehaviour
{

 
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthBar;
    private Animator animator;
    private Vector3 originalScale;
    private bool isAttacking = false;
    public int attackDamage = 20;
    private float attackDelay = 1f; // Delay before the player takes damage
    public int goldReward = 100;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = 1f; // Set initial health bar to full
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isAttacking && !isDead)
        {
            
        }
    }

    
 
    

    

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        // Update the health bar gradually
        healthBar.value = currentHealth / maxHealth;
        animator.SetTrigger("TakeDamage");
        if (currentHealth <= 0)
        {
            Die();
        }
    }



    void Die()
    {

        if (isDead) return;
        isDead = true;
        StopAllCoroutines();
        PlayerController playerController = FindObjectOfType<PlayerController>();
        animator.SetTrigger("Dead");
        if (playerController != null)
        {
            playerController.AddGold(goldReward);
        }
        Destroy(gameObject, 5f);
        Collider2D[] colliders = GetComponents<Collider2D>(); // Added this block
        foreach (Collider2D collider in colliders) // Added this block
        {
            collider.enabled = false; // Added this block
        }

        // Freeze X and Y position
        Rigidbody2D rb = GetComponent<Rigidbody2D>(); // Added this block
        if (rb != null) // Added this block
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; // Added this block
        }

        animator.ResetTrigger("Attack");
        animator.ResetTrigger("TakeDamage");
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
