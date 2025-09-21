using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [Header("Projectile Settings")]
    public int damage = 1;
    public float projectileSpeed = 30f;


    void Update()
    {
        transform.Translate(0, 0, projectileSpeed * Time.deltaTime);
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




