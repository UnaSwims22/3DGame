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

    public void Hurt (int damage)   //health decreases
    {
        if (_isDead) return;

        _health -= damage;
        Debug.Log("Health: " + _health);

        if (_health <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;
       
        Debug.Log("Player has died!");
        gameOverManager.ShowGameOver();   //Show game over UI

    }
}
