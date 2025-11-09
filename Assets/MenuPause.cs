using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
 
    [Header("UI Elements")]
    public GameObject pauseMenuUI;    // The pause panel
    public Button resumeButton;       
    public Button quitButton;

    [Header("Player Control Scripts To Disable")]
    public MonoBehaviour[] scriptsToDisable;
    
    [Header("Settings")]
    public KeyCode pauseKey = KeyCode.Escape;  // Key to pause/resume

    private bool isPaused = false;
    private float cooldownTimer = 0.0f;

    void Start()
    {
        // Hide menu at start
        pauseMenuUI.SetActive(false);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        // Toggle pause when pressing the key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        if (!isPaused) return;

        cooldownTimer -= Time.unscaledDeltaTime;

   

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;    // Freeze the game
        isPaused = true;

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable gameplay scripts (movement, look, shooting, etc.)
        foreach (MonoBehaviour script in scriptsToDisable)
            script.enabled = false;

        // If using PlayerInput
        PlayerInput pi = FindObjectOfType<PlayerInput>();
        if (pi != null)
            pi.DeactivateInput();
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;    // Unfreeze the game
        isPaused = false;

        // Lock cursor for FPS gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable gameplay scripts
        foreach (MonoBehaviour script in scriptsToDisable)
            script.enabled = true;

        // Reactivate PlayerInput for new Input System
        PlayerInput pi = FindObjectOfType<PlayerInput>();
        if (pi != null)
            pi.ActivateInput();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}

