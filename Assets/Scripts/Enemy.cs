using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private GameObject player;
    [SerializeField] private float moveSpeed = 2.5f;
    
    [SerializeField] private int hitPoints = 2;
    private bool isAlive = true;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (isAlive == false)
        {
            Destroy(gameObject);
        }
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
    
    public void TakeDamage()
    {
            hitPoints -= 1;
        if (hitPoints <= 0)
            isAlive = false;
        
        Debug.Log("Enemy damaged and is now at " + hitPoints + " HP.");
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