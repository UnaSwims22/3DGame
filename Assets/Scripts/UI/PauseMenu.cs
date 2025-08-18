using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
                    
        }
    }
     public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  //freezes game
        isPaused = false;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  //unfreezes game
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit pressed");
    }
}
