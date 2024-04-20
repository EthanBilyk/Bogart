using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    
    public void Shoot(float speed, Vector3 direction)
    {
        this.speed = speed;
        this.direction = direction;

    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage();
        }
        else if(other.CompareTag("Enemy"))
        {
            
        }
        Destroy(gameObject);
    }
}
