using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Create the bullet at the fire point position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody2D component from the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Apply force to the bullet to make it move
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }
}
