using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject deathEffect;
    private EnemyDamageFlash damageFlash;
    public EnemyAudio audioPlayer;
   

    void Start()
    {
        currentHealth = maxHealth;

        if (damageFlash == null)
        damageFlash = GetComponent<EnemyDamageFlash>();

        if (audioPlayer == null)
            audioPlayer = GetComponent<EnemyAudio>();
    }

    public void TakeDamage(int amount, Vector3 pushDir = default)
    {
        currentHealth -= amount;

        damageFlash?.Flash();
        audioPlayer?.PlayHurt();

        if (pushDir != Vector3.zero && TryGetComponent(out Rigidbody rb))
            rb.AddForce(pushDir * 7f, ForceMode.Impulse);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
    
    
    



