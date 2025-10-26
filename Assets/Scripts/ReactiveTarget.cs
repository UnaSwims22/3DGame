using UnityEngine;
using System.Collections;
public class ReactiveTarget : MonoBehaviour
{

    private WanderingAI behaviour;

    private void Start()
    {
        behaviour = GetComponent<WanderingAI>();
    }

    public void ReactToHit()
    {
        if (behaviour != null)
        {
            behaviour.SetAlive(false);
        }

        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        // Fall over animation
        transform.Rotate(-75, 0, 0);
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When hit by a RayGun projectile or beam
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            ReactToHit();
        }
    }
}




