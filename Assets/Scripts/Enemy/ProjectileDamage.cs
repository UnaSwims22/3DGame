using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour
{
    public float damage = 10f;     // Default damage
    public string targetTag = "Player"; // The tag that should receive the damage

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with AI who fired it (handled by IgnoreCollision in their scripts)
        if (collision.gameObject.CompareTag(targetTag))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        // Destroy the projectile on impact
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

    



