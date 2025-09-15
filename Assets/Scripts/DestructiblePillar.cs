using UnityEngine;

public class DestructiblePillar : Shootable
{
    public float health = 50f;
    public Rigidbody rb;
    public GameObject fracturedVersion;

    private bool destroyed = false;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Keep it still until it is destroyed
    }

    public override void ReactToHit(Vector3 hitPoint)
    {
        if (destroyed) return;

        health -= 25f; // Damage per shot
        Debug.Log($"Pillar hit! Remaining health: {health}");

        if (health <= 0)
        {
            BreakPillar();
        }
    }

    private void BreakPillar()
    {
        destroyed = true;

        if (fracturedVersion != null)
        {
            Instantiate(fracturedVersion, transform.position, transform.rotation);
            Destroy(gameObject); // Replace with fractured version
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }       
 }

