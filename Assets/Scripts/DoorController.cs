using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{ 
    [SerializeField] private bool isLocked = true;
    private Vector2 index;
    private RoomManagement manager;
    private Player player;
    private float roomSpacing;
    public void LockDoor() {
        isLocked = true;
        // You might want to disable interactions or show the door as locked visually.
        GetComponent<Animator>().SetBool("Locked", true);
    }
    public void UnlockDoor()
    {
        isLocked = false;
        // Enable interactions or update the door's visual state to unlocked.
        GetComponent<Animator>().SetBool("Locked", false);
    }

    // Call this method to check the lock status
    public bool IsLocked()
    {
        return isLocked;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            player = FindObjectOfType<Player>();
        if (!manager)
        {
           manager = FindObjectOfType<RoomManagement>();
        }
        if(isLocked)
            GetComponent<SpriteRenderer>().color = Color.white;
        else if(!isLocked)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int x = 0;
        int y = 0;
        Debug.Log($"Player enetered {gameObject.name}'s trigger");
        if (other.gameObject.CompareTag("Player") && !isLocked)
        {
            if (gameObject.name == "leftDoor(Clone)")
            {
                x = -1;
            }
            else if (gameObject.name == "rightDoor(Clone)")
            {
                x = 1;
            }
            else if (gameObject.name == "topDoor(Clone)")
            {
                y = 1;
            }
            else if (gameObject.name == "bottomDoor(Clone)")
            {
                y = -1;
            }
        }
        Debug.Log($"X: {x} \n Y: {y}");
        travelToNextRoom(x,y);
    }

    public void travelToNextRoom(float x, float y)
    {
        if (!isLocked)
        {
            Vector3 current = player.transform.position;
            Vector3 destination = new Vector3(
                current.x + (x * manager.roomSpacing),
                current.y + (y * manager.roomSpacing),
                0);
            player.transform.position = destination;
        }
    }

    public bool searchForRoom()
    {
        return false;
    }
}
