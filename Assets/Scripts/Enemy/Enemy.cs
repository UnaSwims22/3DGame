using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;

    public void Stun()
    {
        health -= 25f;
        Debug.Log("Enemy stunned!");
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy killed!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 5f)
        {
            Die();
        }
    }
}
