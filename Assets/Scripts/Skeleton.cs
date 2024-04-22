using System;
using UnityEngine;

public class Skeleton : Enemy
{
    private Rigidbody2D rigidBody2D;
    private GameObject player;
    [SerializeField] private float moveSpeed = 2.5f;
    public Transform hand;
    public GameObject weaponPrefab; // Reference to the weapon prefab
    private GameObject currentWeapon; // Reference to the currently held weapon
    
    private float handOffsetDistance = 1.6f;
    public float weaponRadius = 1.6f; // Radius of the weapon rotation around the player
    [SerializeField] private float handOffsetAngle = 1.6f;
    [SerializeField] private float handRadius = 1.6f;

    void Start()
    {
        base.Start(); // Call base class Start method if needed
        // Additional initialization code specific to Skeleton
        base.hitPoints = 40;
        base.isAlive = true;
        rigidBody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        AttachWeaponToHand();
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

    private void Update()
    {
        if (isAlive == false)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void FixedUpdate()
    {
        // Get the position of the player
        Player Player = FindObjectOfType<Player>();
        Vector2 playerRb = Player.GetComponent<Rigidbody2D>().position;

        // Calculate the direction towards the player
        Vector2 direction = (playerRb - (Vector2)transform.position).normalized;

        // Move towards the player
        rigidBody2D.velocity = direction * moveSpeed;

        // Aim and shoot towards the player
        Aim(direction);
        // Rotate the bow based on the hand's rotation
        RotateBow();
        
        
    }
    
    void RotateBow()
    {
        // Copy the hand's rotation to the bow's rotation
        hand.transform.GetChild(0).rotation = hand.rotation;
    }
    
    void Aim(Vector2 direction)
    {
        // Calculate the angle to rotate the hand towards the player
        float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the hand towards the player
        hand.rotation = Quaternion.Euler(0, 0, angleToPlayer);

        // Calculate the position of the hand around the player at a fixed distance
        float angleAroundPlayer = angleToPlayer + handOffsetAngle;
        Vector3 handPosition = transform.position + Quaternion.Euler(0, 0, angleAroundPlayer) * Vector3.right * handRadius;
        hand.position = handPosition;
    }
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage the player
            FindObjectOfType<Player>().TakeDamage();
        }
        
    }
}