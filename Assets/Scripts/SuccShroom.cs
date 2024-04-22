using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccShroom : Enemy
{
    [SerializeField] private float succSpeed = 250f;

    private float succCooldown = 5f;

    private bool isSuccing;

    private float lastSuccTime;

    private float wakeRange = 30f;

    private float endSuccTime;

    private float succDuration = 5f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        hitPoints = 250f;
        isSuccing = false;
        Debug.Log(hitPoints);
    }

    // Update is called once per frame
    void Update()
    {
        if(!player) 
            player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (!isSuccing && Vector2.Distance(transform.position, player.position) <= wakeRange && Time.time - endSuccTime >= succCooldown)
        {
            isSuccing = true;
            Debug.Log("Starting to succ.");
            StartSuccing();
        }
        
        if (!isAlive)
            Die();
    }

    void StartSuccing()
    {
        lastSuccTime = Time.time;
        StartCoroutine(SuccCoroutine());
    }
    
    IEnumerator SuccCoroutine()
    {
        // Apply suction force towards the SuccShroom
        while (isSuccing)
        {
            // Calculate direction towards the SuccShroom
            Vector2 direction = (transform.position - player.position).normalized;

            // Apply suction force to the player
            player.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * succSpeed, ForceMode2D.Force);

            // Wait for the next physics update
            yield return new WaitForFixedUpdate();
            if (Time.time - lastSuccTime >= succDuration)
            {
                isSuccing = false;
                endSuccTime = Time.time;
            }
        }

        // Reset succing state
        isSuccing = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage();
        }
    }
}
