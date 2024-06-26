using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private int _value = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     Debug.Log("Hit the Heart's box");
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         FindObjectOfType<Player>().Heal(_value);
    //         Destroy(gameObject);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit the Health Pots box");
        if (other.gameObject.CompareTag("Player"))
        {
            if (FindObjectOfType<Player>().GetMaxHealth() != FindObjectOfType<Player>().GetHealth())
            {
                FindObjectOfType<Player>().Heal(_value);
                Destroy(gameObject);
            }
        }
    }
}
