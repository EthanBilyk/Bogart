using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    private Rigidbody2D rb2d;
    private Vector3 direction;
    private float range = 10f;

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
        GameObject player = FindObjectOfType<GameObject>();
        Player script = player.gameObject.GetComponent<Player>();
        Move(new Vector3(script.GetPlayerPosition().x, script.GetPlayerPosition().y, 0));
    }
    
    public void Move(Vector3 targetPosition)
    {
        // Calculate the direction towards the target position
        Vector2 direction = (targetPosition - transform.position).normalized;
        rb2d = GetComponent<Rigidbody2D>();

        // Calculate the velocity based on the direction and speed
        Vector2 velocity = direction * speed;

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
        }
        Destroy(gameObject);
        
    }
}
