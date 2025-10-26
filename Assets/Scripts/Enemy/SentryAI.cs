using UnityEngine;


public class SentryAI : MonoBehaviour
{
    [Header("Sentry Settings")]
    public Transform head;              // turret head
    public Transform firePoint;         // Where bullets spawn
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public float rotationSpeed = 45f;   // Degrees per second
    public float detectionRange = 20f;
    public float fireRate = 1f;
    public float stunDuration = 4f;
    public int maxHealth = 3;
    

    [Header("Death Effects")]
    public GameObject deathDustPrefab;

    private Transform player;
    private float fireCooldown = 0f;
    private bool isStunned = false;
    private int currentHealth;
    private Rigidbody rb;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (!this || !gameObject || head == null || player == null) return;
        if (isStunned) return;



        // if player is in range
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (head != null)
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
                if (head != null)
                    head.Rotate(Vector3.up, rotationSpeed * 0.25f * Time.deltaTime);
            }
        }


        void Shoot()
        {
            if (projectilePrefab == null || firePoint == null) return;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * 20f;
            }

            ProjectileDamage projScript = projectile.GetComponent<ProjectileDamage>();
            if (projScript != null)
            {
                projScript.damage = 15f;            // higher damage than wandering AI
                projScript.targetTag = "Player";
            }
               
            Destroy(projectile, 5f); // destroy projectile after 5 sec
        }
    }

    public void TakeDamage(int amount, Vector3 pushDirection)
    {
        currentHealth -= amount;

        

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(StunRoutine());
        }
    }

    

    private System.Collections.IEnumerator StunRoutine()
    {
        isStunned = true;
        Debug.Log("[SentryAI] Stunned for " + stunDuration + " seconds!");
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
        Debug.Log("[SentryAI] Recovered from stun.");
    }

    void Die()
    {
        if (!gameObject) return;
        enabled = false;

        if (deathDustPrefab)
            Instantiate(deathDustPrefab, transform.position, Quaternion.identity);
        Debug.Log("Enemy killed!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the sentry itself hits the ground (ledge collapsed)
        if (collision.gameObject.CompareTag("Ground") && collision.relativeVelocity.magnitude > 5f)
        {
            Debug.Log("[SentryAI] Destroyed by ground impact!");
            Destroy(gameObject);
        }

        if (collision.relativeVelocity.magnitude > 5f)
            if (collision.relativeVelocity.magnitude > 5f)
            {
                if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Ledge"))
                {
                    Die();

                }

                // Die if hit by a falling object 
                if (collision.gameObject.CompareTag("Ledge"))
                {
                    Debug.Log("[SentryAI] Crushed by ledge!");
                    Destroy(gameObject);

                }
            }
    }
}

