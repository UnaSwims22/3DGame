using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameOverManager gameOverManager;
    private int _health;
    private bool _isDead = false;    //player will only "die" once

    [Header("Game Over UI")]
    public GameObject gameOverUI;

    void Start()
    {
        _health = 5;
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public DamageFlash damageFlash;

    public void Hurt (int damage)   //health decreases
    {
        _health -= damage;
        Debug.Log("Health: " + _health);

        if (damageFlash != null)
            damageFlash.Flash();

        if (_health <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;

        GameManager.Instance.ShowGameOver();

        Debug.Log("Player has died!");
        gameOverManager.ShowGameOver();   //Show game over UI

    }
}
