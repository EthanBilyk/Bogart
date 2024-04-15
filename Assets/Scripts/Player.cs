using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private BoxCollider2D collider;
    [SerializeField] private float moveSpeed = 7f;
    private Vector2 movement;
    public GameObject gameOverCanvas;
    
    [SerializeField] private int hitPoints = 3;

    [SerializeField] private int armor = 1;
    private bool isAlive = true;
    private bool isPaused;

    // There may be a better way to set classes, need to think about this one
    // private int class = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector2(horizontal, vertical);

        // Move the player
        rigidBody2D.velocity = movement * moveSpeed;
    }
    
    void Update()
    {
        if (!isAlive && !isPaused)
        {
            // Pause the game
            Time.timeScale = 0;
            isPaused = true;

            // Activate the game over canvas
            gameOverCanvas.SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides with something, stop their movement
        rigidBody2D.velocity = Vector2.zero;
        movement = new Vector2(0,0);
    }
    

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void AddMoveSPeed(float add)
    {
        moveSpeed = moveSpeed + add;
    }

    public void MultMoveSpeed(float mult)
    {
        moveSpeed = moveSpeed * mult;
    }

    public void TakeDamage()
    {
        if (armor > 0)
        {
            armor -= 1;
        }
        else
        {
            hitPoints -= 1;
        }
        if (hitPoints <= 0)
            isAlive = false;
        
        Debug.Log("You took damage and are now at " + hitPoints + " HP.");
    }

    public void Heal(int heal)
    {
        hitPoints += heal;
    }

    public void GainArmor(int armorVal)
    {
        armor += armorVal;
    }

    public float GetHealth()
    {
        return hitPoints;
    }

    public int GetArmor()
    {
        return armor;
    }

    public Vector2 GetPlayerPosition()
    {
        return rigidBody2D.position;
    }

    public bool getPaused()
    {
        return isPaused;
    }

    public void setPaused(bool paused)
    {
        isPaused = paused;
    }

    public bool getAlive()
    {
        return isAlive;
    }

    public void setAlive(bool alive)
    {
        isAlive = alive;
    }
    
}
