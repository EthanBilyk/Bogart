using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public float cooldownTime = 0.5f;
    [SerializeField] public GameObject bulletSpawn;
    private float lastShootTime;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time - lastShootTime >= cooldownTime)
        {
            Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            Shoot(direction);
        }
    }

    public void Shoot(Vector3 targetPosition)
    {
        // Instantiate the projectile at the weapon's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, bulletSpawn.transform.position, transform.rotation);
        projectile.transform.rotation = new Quaternion(transform.rotation.x * -1, transform.rotation.y * -1, transform.rotation.z * -1, 0);

        // Access the Projectile component of the instantiated projectile
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        

        if (projectileComponent != null)
        {
            // Set the initial velocity of the projectile based on the direction
            projectileComponent.Move(targetPosition);
            lastShootTime = Time.time;
        }
        else
        {
            Debug.LogError("Projectile component not found on instantiated projectile!");
        }
    }
    public void AddPercentAttackSpeed(float attackSpeed)
    {
        // Calculate the percentage change in attack speed
        float percentChange = 1 + attackSpeed; // If attackSpeed is 0.1, percentChange will be 1.1

        // Adjust the cooldown time proportionally
        cooldownTime /= percentChange;
        Debug.LogError("Attack speed is " + cooldownTime);
    }
}