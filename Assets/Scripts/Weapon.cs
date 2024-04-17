using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public float cooldownTime = 0.5f;
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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

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
}