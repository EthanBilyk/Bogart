using System.Collections;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int speed = 6;
    public float cooldownTime = 0.5f; // Cooldown time between shots
    private float baseCooldownTime = 0.5f;
    private float lastShootTime; // Time when the last shot was fired
    private float bulletOffsetDistance = 0.6f;

    private void FixedUpdate()
    {
        
    }

    public void AddPercentAttackSpeed(float attackSpeed)
    {
        this.cooldownTime = baseCooldownTime + (attackSpeed * baseCooldownTime);
    }

    private void Update()
    {
        // Check if the player is holding down the fire button
        if (Input.GetButton("Fire1"))
        {
            // Check if enough time has passed since the last shot to allow shooting again
            if (Time.time - lastShootTime >= cooldownTime)
            {
                Shoot(); // Fire a bullet
                lastShootTime = Time.time; // Update the last shot time
            }
        }
    }
    public void Shoot()
    {
        // Get the direction from the pistol's position to the mouse cursor
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        // Calculate the bullet offset based on the direction
        Vector3 bulletOffset = direction * bulletOffsetDistance;

        // Instantiate the bullet at the pistol's position with the offset
        GameObject bullet = Instantiate(bulletPrefab, transform.position + bulletOffset, transform.rotation);

        // Get the bullet script component
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            // Set the bullet's speed and direction
            bulletScript.Shoot(speed, transform.right);
        }
        else
        {
            Debug.LogError("Bullet script component not found!");
        }
    }


}