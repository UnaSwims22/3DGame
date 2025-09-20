using UnityEngine;


public class SentryAI : MonoBehaviour
{
    public Transform head;              // turret head
    public Transform firePoint;         // Where bullets spawn
    public GameObject projectilePrefab; // Bullet prefab
    public float rotationSpeed = 45f;   // Degrees per second
    public float detectionRange = 20f;
    public float fireRate = 1f;
    private float fireCooldown = 0f;
    private Transform player;


    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
        }
        else
        {
            Debug.LogWarning("[SentryAI] No GameObject with tag 'Player' found in the scene. Make sure your player GameObject has the 'Player' tag.");
        }
        
    }

    void Update()
    {
        if (player == null) return;

        // Check if player is in range
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // Rotate head to face player
            Vector3 direction = player.position - head.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            head.rotation = Quaternion.RotateTowards(head.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Shoot when cooldown allows
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
        }
        else
        {
           
            head.Rotate(Vector3.up, rotationSpeed * 0.25f * Time.deltaTime);
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * 500f, ForceMode.Impulse);
        }

        Destroy(projectile, 5f); // Auto-destroy projectile after 5 sec
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the sentry itself hits the ground (ledge collapsed)
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
