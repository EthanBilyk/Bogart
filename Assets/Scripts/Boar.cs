using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Boar : Enemy
{ 
        private float initialSpeed = 8f; // Initial speed of the boar
        private float maxSpeed = 25f; // Maximum speed of the boar
        private float acceleration = 10f; // Acceleration rate of the boar
        private float currentSpeed; // Current speed of the boar
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private Collider2D collider;
        private Vector2 chargeDirection; // Direction of the charge attack
        private bool isCharging = false; // Flag to indicate if the boar is currently charging
        private float attackRange = 15f;
        private float stunTime;
        private bool isStunned;
        private float stunDuration = 3f; // Duration of stun in seconds
        private float stunEndTime; // Time when the stun ends
        private void Start()
        {
                base.Start(); // Call base class Start method if needed
                // Additional initialization code specific to Boar
                base.hitPoints = 200;
                base.isAlive = true;
                rb2d = GetComponent<Rigidbody2D>();
                collider = GetComponent<Collider2D>();
                player = GameObject.FindGameObjectWithTag("Player").transform;
                currentSpeed = initialSpeed;
                isStunned = false;
                isCharging = false;
        }

        private void Update()
        {
                base.Update();

        }

        private void FixedUpdate()
        {
                if (isStunned)
                {
                        currentSpeed = initialSpeed;
                        // Check if stun duration has elapsed
                        if (Time.time >= stunEndTime)
                        {
                                // End stun
                                isStunned = false;
                                isCharging = false;
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
                        else if(distanceToPlayer > attackRange)
                                rb2d.velocity = (player.position - transform.position).normalized * 4f;
                }

                
                MoveAttack();
        }

        private void MoveAttack()
        {
                if (isCharging && !isStunned)
                {
                        // Gradually increase the speed until it reaches the maximum speed
                        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

                        // Move the boar in the charge direction with the current speed
                        rb2d.velocity = chargeDirection * currentSpeed;
                }
                else if(Vector2.Distance(transform.position, player.position) <= attackRange && !isStunned)
                {
                        rb2d.velocity = (player.position - transform.position).normalized * initialSpeed;
                }
                else if (isStunned)
                {
                        // Reset speed to initial speed
                        currentSpeed = initialSpeed;
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
                Debug.Log("Charging");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
                Debug.Log(other.gameObject.tag + " entered trigger");
                if (other.gameObject.CompareTag("Environment"))
                {
                        // If the boar collides with the environment, and it's not already stunned
                        rb2d.velocity = Vector2.zero;
                        stunEndTime = Time.time + stunDuration; // Set the time when the stun ends
                        isStunned = true; // Start stun
                        currentSpeed = initialSpeed;
                }
                if (other.gameObject.CompareTag("Player"))
                {
                        player.GetComponent<Player>().TakeDamage();
                }
        }
        
        
}
    