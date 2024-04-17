using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private GameObject player;
    [SerializeField] private float moveSpeed = 2.5f;
    
    [SerializeField] private int hitPoints = 2;
    private bool isAlive = true;
    public Transform hand;
    public GameObject weaponPrefab; // Reference to the weapon prefab
    private GameObject currentWeapon; // Reference to the currently held weapon
    
    private float handOffsetDistance = 1.6f;
    public float weaponRadius = 1.6f; // Radius of the weapon rotation around the player
    [SerializeField] private float handOffsetAngle;
    [SerializeField] private float handRadius;

    void Start()
    {
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
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // Get the position of the player
        Vector2 playerPosition = player.transform.position;

        // Calculate the direction towards the player
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;

        // Move towards the player
        rigidBody2D.velocity = direction * moveSpeed;

        // Aim and shoot towards the player
        AimAndShoot(direction);
    }
    
    void AimAndShoot(Vector2 direction)
    {
        // Calculate the angle to rotate the hand towards the player
        float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        hand.rotation = Quaternion.Euler(0, 0, angleToPlayer);

        // Calculate the position of the hand around the player at a fixed distance
        float angleAroundPlayer = angleToPlayer + handOffsetAngle;
        Vector3 handPosition = transform.position + Quaternion.Euler(0, 0, angleAroundPlayer) * Vector3.right * handRadius;
        hand.position = handPosition;

        // Shoot towards the player
        Weapon weapon = currentWeapon.GetComponent<Weapon>();
        weapon.Shoot(direction);
    }
    
    public void TakeDamage()
    {
            hitPoints -= 1;
        if (hitPoints <= 0)
            isAlive = false;
        
        Debug.Log("Enemy damaged and is now at " + hitPoints + " HP.");
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