using UnityEngine;

public class LedgeDestruction : MonoBehaviour
{
    public float destroyDelay = 0.1f; // Short delay before destroying

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy only if it collides with the ground layer
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Destroy the ledge
            Destroy(gameObject, destroyDelay);
        }
    }
}
