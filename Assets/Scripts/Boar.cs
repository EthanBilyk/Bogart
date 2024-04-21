using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Boar : MonoBehaviour
{ 
        private int hp = 200; 
        private float initialSpeed = 20f; // Initial speed of the boar
        private float maxSpeed = 100f; // Maximum speed of the boar
        private float acceleration = 4f; // Acceleration rate of the boar
        private float currentSpeed; // Current speed of the boar
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private Collider2D collider;
        private Vector2 chargeDirection; // Direction of the charge attack
        private bool isCharging = false; // Flag to indicate if the boar is currently charging
        private Transform player; // Reference to the player's transform
        private float attackRange = 15f;
        private float stunTime;
        private bool isStunned;
        private float stunDuration = 3f; // Duration of stun in seconds
        private float stunEndTime; // Time when the stun ends
        private void Start()
        {
                rb2d = GetComponent<Rigidbody2D>();
                collider = GetComponent<Collider2D>();
                player = GameObject.FindGameObjectWithTag("Player").transform;
                currentSpeed = initialSpeed;
                isStunned = false;
        }

        private void Update()
        {
                if(!player) 
                        player = GameObject.FindGameObjectWithTag("Player").transform;
            
        }

        private void FixedUpdate()
        {
                if (isStunned)
                {
                        // Check if stun duration has elapsed
                        if (Time.time >= stunEndTime)
                        {
                                // End stun
                                isStunned = false;
                                isCharging = false;
                                currentSpeed = initialSpeed;
                        }
                }
                else if (!isCharging)
                {
                        // Calculate the distance between the boar and the player
                        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
                        // Check if the player is within attack range
                        if (distanceToPlayer <= attackRange)
                        {
                                // If not already charging, snapshot the direction of the player for charge attack
                                chargeDirection = (player.position - transform.position).normalized;
                                StartCharge();
                        }
                }

                MoveAttack();
        }

        private void MoveAttack()
        {
                if (isCharging)
                {
                        Debug.Log("Charging");
                        // Gradually increase the speed until it reaches the maximum speed
                        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);

                        // Move the boar in the charge direction with the current speed
                        rb2d.velocity = chargeDirection * currentSpeed;

                        Debug.Log("Velocity: " + rb2d.velocity); // Check if velocity is changing
                }
                else
                {
                        // If not charging, stop the boar
                        rb2d.velocity = Vector2.zero;
                }
        }
        
        // Method to initiate the charge attack
        public void StartCharge()
        {
                isCharging = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
                // If the boar collides with the environment and it's not already stunned
                isCharging = false;
                rb2d.velocity = Vector2.zero;
                stunEndTime = Time.time + stunDuration; // Set the time when the stun ends
                isStunned = true; // Start stun
        }
}
    