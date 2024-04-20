using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float speed;
    private Vector3 direction;
    private float range = 10f;

    private void Update()
    {
        // Move projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
    
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        GameObject player = FindObjectOfType<GameObject>();
        Player script = player.gameObject.GetComponent<Player>();
    }
    
    public void Shoot(float speed, Vector3 direction)
    {
        if (rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();
        
        // Calculate the velocity based on the direction and speed
        this.speed = speed;
        this.direction = direction;
        Vector2 velocity = direction.normalized * speed;

        // Apply the velocity to the Rigidbody2D
        rb2d.velocity = velocity;

    }
    
    
    public void Move(Vector3 targetPosition)
    {
        // Calculate the direction towards the target position
        Vector2 direction = (targetPosition - transform.position).normalized;

        // Calculate the velocity based on the direction and speed
        Vector3 velocity = direction * speed;

        if (rb2d != null)
        {
            // Apply the velocity to the rigidbody
            rb2d.velocity = velocity;
        }
        else
        {
            Debug.LogError("Rigidbody component not found on the projectile!");
        }
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage();
            Destroy(gameObject);
        }
    }
}
