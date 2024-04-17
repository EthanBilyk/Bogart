using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedPickup : MonoBehaviour
{
    private float value = .1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Hit the AttackSpeedPickup");
        if (other.gameObject.CompareTag("Player"))
        {
            Player playerScript = other.gameObject.GetComponent<Player>();
            playerScript.AddPercentAttackSpeed(value);
            Destroy(gameObject);
        }
    }
}
