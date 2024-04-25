using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleSlime : Enemy
{
    public Transform player;
    public float jumpForce = 10f;

    void Update() {
        Vector3 direction = (player.position - transform.position).normalized;
        if (IsGrounded()) {
            GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, 1, direction.z) * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }
}
