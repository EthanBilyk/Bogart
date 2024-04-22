using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Projectile script
public class Projectile : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    private Vector3 direction;
    [SerializeField] public float damage;

    private void Update()
    {
        // Move projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
    public void Shoot(float speed, Vector3 direction)
    {
        this.speed = speed;
        this.direction = direction;

    }
    
    private void Start()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Call the Move method with the mouse position
        Move(mousePosition);
    }

    public void Move(Vector3 targetPosition)
    {
        // Calculate the direction towards the target position
        Vector2 direction = (targetPosition - transform.position).normalized;
        Debug.Log("Projectile has direction: " + direction + ", and speed: " + speed);

        // Calculate the velocity based on the direction and speed
        Vector2 velocity = direction.normalized * speed;

        // Get the rigidbody component of the projectile
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Apply the velocity to the rigidbody
            rb.velocity = velocity;
        }
        else
        {
            Debug.LogError("Rigidbody component not found on the projectile!");
        }
    }

    public void Init(Vector3 direction)
    {
        // Set initial velocity based on direction
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    { 
        if(other.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Environment"))
            Destroy(gameObject);
    }
}
