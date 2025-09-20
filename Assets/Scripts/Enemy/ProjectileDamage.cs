using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit for " + damage + "damage!");
            // subtract player health 
        }

        Destroy(gameObject);
    }
}
