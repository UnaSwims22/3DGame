using UnityEngine;

public class DestructibleLedge : MonoBehaviour
{
    
    [Header("Ledge Settings")]
    public float destroyDelay = 1f;        // How long after impact before destroying
    public float fallKillVelocity = 2f;    // Minimum speed to kill enemies 
    private Rigidbody rb;
    private bool hasFallen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;  // Start stable
        rb.isKinematic = true;  // Only falls when "triggered"
    }

    public void DropLedge()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        hasFallen = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasFallen) return;

        //Crush Sentry AI
        if (collision.collider.CompareTag("Enemy"))
        {
            // Check if we hit it hard enough
            if (collision.relativeVelocity.magnitude >= fallKillVelocity)
            {
                SentryAI ai = collision.collider.GetComponent<SentryAI>();
                if (ai != null)
                {
                    ai.TakeDamage(ai.maxHealth); // Instantly kills AI
                }
            }
        }

        //Destroy ledge when it hits the ground
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("[Ledge] Hit ground, destroying...");
            Destroy(gameObject, destroyDelay);
        }
    }
}


