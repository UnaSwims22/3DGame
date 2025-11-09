using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button quitButton;

    [Header("Player Reference")]
    public GameObject player;


    private bool isGameOver = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
    }

    
    public void TriggerGameOver()
    {
        Debug.Log(" TriggerGameOver() CALLED");

        if (isGameOver) return;
        isGameOver = true;

        // Show game over UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log(" Game Over Panel SET ACTIVE");
        }
        else 
        {
            Debug.LogError(" GameOverPanel is NOT assigned!");
        }

        if (player != null)
        {
            var controller = player.GetComponent<FPController>();
            if (controller != null) controller.enabled = false;
        }


    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName: "MainMenu");
        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        
    }
}
  