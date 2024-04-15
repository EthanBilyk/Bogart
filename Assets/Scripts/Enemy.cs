using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private GameObject player;
    [SerializeField] private float moveSpeed = 2.5f;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        // Get the position of the player
        Vector2 playerPosition = FindObjectOfType<Player>().GetPlayerPosition();

        // Calculate the direction towards the player
        Vector2 direction = (playerPosition - rigidBody2D.position).normalized;

        // Move towards the player
        rigidBody2D.velocity = direction * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage the player
            FindObjectOfType<Player>().TakeDamage();
        }
    }
}