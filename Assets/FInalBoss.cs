using System.Collections;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public float health = 100f;
    public float enragedHealthThreshold = 50f;
    public float enragedFireRateMultiplier = 2f;
    public Transform[] firePoints;
    public GameObject projectilePrefab;
    public GameObject explosionEffect;
    public float fireRate = 1f;
    private bool isEnraged = false;
    private bool isFiring = false;

    private void Start()
    {
        StartCoroutine(FireProjectiles());
    }

    private void Update()
    {
        if (health <= enragedHealthThreshold && !isEnraged)
        {
            Enrage();
        }
    }

    private void Enrage()
    {
        isEnraged = true;
        fireRate *= enragedFireRateMultiplier; // Increase the firing rate
        Debug.Log("Boss is enraged! Firing rate increased.");
    }

    private IEnumerator FireProjectiles()
    {
        while (health > 0)
        {
            foreach (Transform firePoint in firePoints)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                // Add additional logic to control the projectile's behavior here if needed
            }

            yield return new WaitForSeconds(fireRate);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Boss took damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss is defeated!");
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
