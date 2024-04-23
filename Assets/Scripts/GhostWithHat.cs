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

    }

    private void FixedUpdate()
    {
        if (player)
        {
            Vector2 directionToPlayer = player.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer < keepDistance)
            {
                Vector2 desiredPosition = (Vector2)transform.position -
                                          directionToPlayer.normalized * (keepDistance - distanceToPlayer);
                transform.position = Vector2.MoveTowards(transform.position, desiredPosition,
                    Time.fixedDeltaTime * movementSpeed);
            }
            else if (distanceToPlayer > keepDistance)
            {
                Vector2 desiredPosition = (Vector2)transform.position +
                                          directionToPlayer.normalized * (keepDistance + distanceToPlayer);
                transform.position = Vector2.MoveTowards(transform.position, desiredPosition,
                    Time.fixedDeltaTime * movementSpeed);
            }

            if (distanceToPlayer < fireballRange && Time.time - lastCastTime > fireballCooldown)
            {
                castFireball(directionToPlayer.normalized);
            }
        }
    }

    private void castFireball(Vector2 direction)
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireballSpeed; // Set velocity towards the player
        lastCastTime = Time.time;
    }
}
