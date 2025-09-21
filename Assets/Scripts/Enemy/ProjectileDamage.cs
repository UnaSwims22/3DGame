using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [Header("Projectile Settings")]
    public int damage = 1;
    public float projectileSpeed = 30f;


    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // prevents fast projectiles from passing through
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Hurt(damage);  // triggers DamageFlash and GameOver
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCharacter pc = collision.collider.GetComponent<PlayerCharacter>();
            if (pc != null)
            {
                pc.Hurt(damage);
                Debug.Log("Player hit for " + damage + "damage!");
            } 
        }

        Destroy(this.gameObject);
    }
}




