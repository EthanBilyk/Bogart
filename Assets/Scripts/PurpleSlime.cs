using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class PurpleSlime : Enemy
{
    public float jumpForce = 10f;
    public float lastLandTime;
    private float jumpCooldown = 2f;
    private ParticleSystem system;
    private Rigidbody rb2D;
    private bool isGrounded;

    private new void Start()
    {
        isGrounded = true;
        base.Start();
        base.hitPoints = 140;
        if(!system)
            system = GetComponent<ParticleSystem>();
        if (!rb2D)
            rb2D = GetComponent<Rigidbody>();
    }

    private new void Update()
    {
        base.Update();
        IsGrounded();
    }

    void FixedUpdate() {
        Vector3 direction = (player.position - transform.position);
        direction.y = 0;  // Ignore the vertical difference when calculating direction
        direction = direction.normalized;  // Normalize the horizontal direction

        if (isGrounded && Time.time - lastLandTime > jumpCooldown) {
            // Apply horizontal force along with a fixed upward force for jumping
            rb2D.AddForce(new Vector2(direction.x, 1) * jumpForce, ForceMode.Impulse);
        }
    }

    void IsGrounded() {
        Debug.Log($"{isGrounded}");
        isGrounded =  Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            player.gameObject.GetComponent<Player>().TakeDamage();
    }
}
