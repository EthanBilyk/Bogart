using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{ 
    [SerializeField] private bool isLocked = true;
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
        
    }
}
