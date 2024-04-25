using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWithHat : Enemy
{
    [SerializeField] private GameObject fireballPrefab;

    private float keepDistance = 15f;

    private float movementSpeed = 10f;
    private float lastCastTime;
    private float fireballCooldown = 5f;
    private float fireballRange = 25f;
    private float fireballSpeed = 20f;
    private bool isStuck = false;
    private float stuckStart;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        hitPoints = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if(!isAlive)
            Die();

    }

    private void FixedUpdate()
    {
        if (player)
        {
            Vector2 directionToPlayer = player.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            // Check for obstacles
            if (isStuck)
            {
                // Calculate a new direction to move in (for example, rotate clockwise)
                Vector2 newDirection = Quaternion.Euler(0, 0, 45) * directionToPlayer.normalized;
                // Move towards the new direction
                transform.position = Vector2.MoveTowards(transform.position,
                    (transform.position + (Vector3)newDirection), Time.fixedDeltaTime * movementSpeed);
                if (Time.time - stuckStart > 3f)
                    isStuck = false;
            }
            //run away
            else if (distanceToPlayer < keepDistance)
            {
                Vector2 desiredPosition = (Vector2)transform.position -
                                          directionToPlayer.normalized * (keepDistance - distanceToPlayer);
                transform.position = Vector2.MoveTowards(transform.position, desiredPosition,
                    Time.fixedDeltaTime * movementSpeed);
            }
            //run closer
            else if (distanceToPlayer > keepDistance)
            {
                Vector2 desiredPosition = (Vector2)transform.position +
                                          directionToPlayer.normalized * (keepDistance + distanceToPlayer);
                transform.position = Vector2.MoveTowards(transform.position, desiredPosition,
                    Time.fixedDeltaTime * movementSpeed);
            }

            if (distanceToPlayer < fireballRange && Time.time - lastCastTime > fireballCooldown && fireballPrefab != null)
            {
                castFireball(directionToPlayer.normalized);
            }
        }
    }

    private void castFireball(Vector2 direction)
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fireball.transform.localScale.Set(6f,6f,0f);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireballSpeed; // Set velocity towards the player
        lastCastTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            player.GetComponent<Player>().TakeDamage();
        if (other.gameObject.CompareTag("Environment"))
        {
            isStuck = true;
            stuckStart = Time.time;
        }
    }
}
