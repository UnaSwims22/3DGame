using UnityEngine;
using UnityEngine.Rendering;

public class LedgeDestruction : MonoBehaviour
{
    public float destroyDelay = 0.1f; // Short delay before destroying
    public float crushForce = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject); // Destroy enemy immediately
        }

        // Destroy only if it collides with the ground layer
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("[Ledge] Hit ground, destroying.");
            Destroy(gameObject, destroyDelay);
        }
    }
}
