using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Common properties for all enemies
    protected float hitPoints;
    protected GameObject player;
    protected float armor;
    protected bool isAlive;
    protected Vector2 spawnLocation;
    

    // Common methods for all enemies
    protected virtual void Start()
    {
        // Initialization code common to all enemies
        isAlive = true;
    }

    protected void Update()
    {
        if(!isAlive)
            Die();
    }

    // Method to take damage (can be overridden by specific enemy types)
    public virtual void TakeDamage(float damage)
    {
        if (armor > 0)
        {
            armor -= damage;
        }
        else
        {
            hitPoints -= damage;
        }
        if (hitPoints <= 0)
            isAlive = false;
        
        Debug.Log("The enemy took damage and is now at: " + hitPoints + " hp.");
    }

    // Method to be called when the enemy dies (can be overridden)
    protected virtual void Die()
    {
        // Clean-up code, such as destroying the enemy GameObject
        Destroy(gameObject);
    }
}