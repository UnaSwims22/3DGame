using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject deathEffect;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
    
    
    



