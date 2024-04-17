using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab;

    public int speed = 4;
    public float cooldownTime = 0.8f; // Cooldown time between shots
    private float baseCooldownTime = 0.8f;
    private float lastShootTime; // Time when the last shot was fired
    private float bulletOffsetDistance = 0.6f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastShootTime >= cooldownTime)
        {
            Shoot(); // Fire a bullet
            lastShootTime = Time.time; // Update the last shot time
        }
        
    }
    
    public void Shoot()
    {
        // Get the direction from the pistol's position to the mouse cursor
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        // Calculate the bullet offset based on the direction
        Vector3 bulletOffset = direction * bulletOffsetDistance;

        // Instantiate the bullet at the pistol's position with the offset
        GameObject arrow = Instantiate(arrowPrefab, transform.position + bulletOffset, transform.rotation);

        // Get the bullet script component
        Bullet arrowScript = arrow.GetComponent<Bullet>();

        if (arrowScript != null)
        {
            // Set the bullet's speed and direction
            arrowScript.Shoot(speed, transform.right);
        }
        else
        {
            Debug.LogError("Bullet script component not found!");
        }
    }
}
