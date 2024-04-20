using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public float cooldownTime = 1.2f;
    [SerializeField] public float spawnDistance = 1.0f;
    private float lastShootTime;
    [SerializeField] private float range = 10f;
    private Transform playerTransform;
    [SerializeField] private float speed = 6f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Check if enough time has passed since the last shot
            // Find all nearby players within the specified range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D collider in colliders)
            {
                // Check if the collider belongs to a player
                if (collider.CompareTag("Player") && Time.time - lastShootTime >= cooldownTime)
                {
                    // Calculate direction towards the player
                    Player Player = FindObjectOfType<Player>();
                    Vector2 playerRb = Player.GetComponent<Rigidbody2D>().position;

                    // Calculate the direction towards the player
                    Vector2 direction = (playerRb - (Vector2)transform.position).normalized;

                    // Shoot towards the player
                    Shoot(direction);
                    
                    // Exit the loop after shooting at the first player found
                    break;
                }
            }
    }

    public void Shoot(Vector2 direction)
    {
        GameObject obj = GameObject.FindWithTag("Enemy");
        Enemy enemyScript = obj.GetComponent<Enemy>();
        Vector3 handPosition = enemyScript.hand.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Instantiate the projectile at the hand position and rotation
        GameObject projectile = Instantiate(projectilePrefab, handPosition, Quaternion.Euler(0f, 0f, angle));
        

        // Access the Projectile component of the instantiated projectile
        EnemyProjectile projectileComponent = projectile.GetComponent<EnemyProjectile>();

        if (projectileComponent != null)
        {
            // Set the initial velocity of the projectile based on the direction
            projectileComponent.Shoot(speed, direction);
            this.lastShootTime = Time.time;
        }
        else
        {
            Debug.LogError("Projectile component not found on instantiated projectile!");
        }
    }


}
