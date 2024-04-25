using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private CapsuleCollider2D collider;
    
    [SerializeField] private float moveSpeed = 7f;
    private float initialMoveSpeed;
    private Vector2 movement;
    public GameObject gameOverCanvas;
    
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int maxHitPoints = 3;

    [SerializeField] private int armor = 0;
    private bool isAlive = true;
    private bool isPaused;
    public Transform hand; // Reference to the hand GameObject where the pistol will be held
    public GameObject weaponPrefab; // Reference to the weapon prefab
    private GameObject currentWeapon; // Reference to the currently held weapon

    private float handOffsetDistance = 1.6f;
    public float weaponRadius = 1.6f; // Radius of the weapon rotation around the player
    [SerializeField] private float handOffsetAngle;
    [SerializeField] private float handRadius;
    
    private bool isInvincible = false; // Flag to indicate if the player is currently invincible
    private float invincibilityDuration = 1f; // Duration of invincibility frames in seconds
    private float invincibilityEndTime; // Time when invincibility frames end

    // There may be a better way to set classes, need to think about this one
    // private int class = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        // Instantiate the pistol and attach it to the hand
        AttachWeaponToHand();
        gameOverCanvas = GetComponentInChildren<GameObject>();
        gameOverCanvas.SetActive(false);
        initialMoveSpeed = moveSpeed;
    }
    
    void AttachWeaponToHand()
    {
        // Instantiate the pistol and attach it to the hand
        currentWeapon = Instantiate(weaponPrefab, hand.position, hand.rotation, hand);
        if (currentWeapon != null)
        {
            Debug.Log("Weapon attached to hand successfully!");
        }
        else
        {
            Debug.LogError("Failed to attach weapon to hand!");
        }
    }

    public void AddPercentAttackSpeed(float attackSpeed)
    {
        Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
        weaponScript.AddPercentAttackSpeed(attackSpeed);
    }
    
    void Aim()
    {
        // Calculate the angle to rotate the hand towards the mouse cursor
        Vector3 directionToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Rotate the hand towards the mouse cursor
        hand.rotation = Quaternion.Euler(0, 0, angleToMouse);

        // Calculate the position of the hand around the player at a fixed distance
        float angleAroundPlayer = angleToMouse + handOffsetAngle; // Add an offset angle if needed
        Vector3 handPosition = transform.position + Quaternion.Euler(0, 0, angleAroundPlayer) * Vector3.right * handRadius;
        hand.position = handPosition;
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        // Move the player
        rigidBody2D.velocity = movePlayer() * moveSpeed;
        hand.position = new Vector3(rigidBody2D.position.x - 0.6f, rigidBody2D.position.y, 0);
        
        // Aim the pistol towards the mouse cursor
        Aim();

        // Fire the pistol when the fire button is pressed
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }
    
    void Fire()
    {
        // Check if the current pistol exists
        if (currentWeapon != null)
        {
            // Get the pistol's script component
            Weapon weapon = currentWeapon.GetComponent<Weapon>();

            // Check if the pistol script exists and fire the pistol
            if (weapon != null)
            {
                // Get the mouse position in world coordinates
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Pass the mouse position to the Shoot method
                weapon.Shoot(mousePosition);
            }
        }
    }

    private Vector2 movePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector2(horizontal, vertical);
        return movement;
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
        
        if (isInvincible && Time.time >= invincibilityEndTime)
        {
            // End invincibility frames
            isInvincible = false;
            // Implement any visual feedback to indicate invincibility has ended
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
        initialMoveSpeed = moveSpeed;
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
        if (!isInvincible) // Check if the player is not currently invincible
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

            // Start invincibility frames
            StartInvincibility();
        }
    }
    
    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityEndTime = Time.time + invincibilityDuration;
        // Implement any visual feedback here (e.g., flashing)
    }

    public void Heal(int heal)
    {
        if (hitPoints < maxHitPoints)
        {
            hitPoints += heal;
            Debug.Log("You Healed and are now at " + hitPoints + " HP.");
        }
    }
    
    public void increaseHP(int hp)
    {
        maxHitPoints += hp;
        Debug.Log("You increased your max HP and are now at " + maxHitPoints + " HP.");
    }

    public void GainArmor(int armorVal)
    {
        armor += armorVal;
    }

    public int GetHealth()
    {
        return hitPoints;
    }
    
    public int GetMaxHealth()
    {
        return maxHitPoints;
    }

    public int GetArmor()
    {
        return armor;
    }

    public Vector2 GetPlayerPosition()
    {
        return new Vector2(rigidBody2D.position.x, rigidBody2D.position.y);
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
