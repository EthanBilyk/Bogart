using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public float cooldownTime = 0.5f;
    [SerializeField] public float spawnDistance = 1.0f;
    private float lastShootTime;
    private float range = 10f;

    private void Update()
    {
        // Check if enough time has passed since the last shot
        if (Time.time - lastShootTime >= cooldownTime)
        {
            // Find all nearby players within the specified range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D collider in colliders)
            {
                // Check if the collider belongs to a player
                if (collider.CompareTag("Player"))
                {
                    // Calculate direction towards the player
                    Vector3 direction = (collider.transform.position - transform.position).normalized;
                    
                    // Shoot towards the player
                    Shoot(direction);
                    
                    // Exit the loop after shooting at the first player found
                    break;
                }
            }
        }
    }

    public void Shoot(Vector3 targetPosition)
    {
        // Instantiate the projectile at the weapon's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Access the Projectile component of the instantiated projectile
        Projectile projectileComponent = projectile.GetComponent<Projectile>();

        if (projectileComponent != null)
        {
            // Set the initial velocity of the projectile based on the direction
            projectileComponent.Move(targetPosition*spawnDistance);
            lastShootTime = Time.time;
        }
        else
        {
            Debug.LogError("Projectile component not found on instantiated projectile!");
        }
    }
}
