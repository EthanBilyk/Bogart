using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class PurpleSlime : Enemy
{
    public float jumpForce = 1f;
    public float lastLandTime;
    private float jumpCooldown = 5f;
    private ParticleSystem system;
    private Rigidbody2D rb2D;

    private new void Start()
    {
        base.Start();
        base.hitPoints = 140;
        if(!system)
            system = GetComponentInChildren<ParticleSystem>();
        if (!rb2D)
            rb2D = GetComponent<Rigidbody2D>();
    }

    private new void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {
        if (Time.time - lastLandTime > jumpCooldown)
            Jump();
    }
    
    public void Jump()
    {
            rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            lastLandTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            player.gameObject.GetComponent<Player>().TakeDamage();
    }

    IEnumerator jumpAttack()
    {
        
        yield break;
    }
}
