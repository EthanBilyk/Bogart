using System;
using Unity.VisualScripting;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public int speed = 6;
    private void Start()
    {
        bulletPrefab = GetComponent<GameObject>();
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            GameObject bullet = Instantiate(bulletPrefab);
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>());
            bullet.transform.position = bulletSpawn.position;
            Vector3 rotation = bullet.transform.rotation.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward * speed, ForceMode.Impulse);
            StartCoroutine("DestroyProjectile");
        }
    }
}