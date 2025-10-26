using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject deathEffect;
    private EnemyDamageFlash damageFlash;

    void Start()
    {
        currentHealth = maxHealth;
        damageFlash = GetComponent<EnemyDamageFlash>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (damageFlash != null)
            damageFlash.Flash();

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
    
    
    



