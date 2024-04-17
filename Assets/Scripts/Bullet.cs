using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private Vector3 direction;

    public void Shoot(float speed, Vector3 direction)
    {
        this.speed = speed;
        this.direction = direction;

    }

    private void Update()
    {
        // Move the bullet forward using raycasting
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, speed * Time.deltaTime))
        {
            // If the ray hits something, move the bullet to the point of collision
            transform.position = hit.point;
            // Perform actions based on the object hit
            if (hit.collider.CompareTag("Player"))
            {
                // Handle player hit
            }

            if (hit.collider.CompareTag("Enemy"))
            {
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
        else
        {
            // If the ray doesn't hit anything, move the bullet forward
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
        else if(other.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage();
        }
        Destroy(gameObject);
        
    }
}