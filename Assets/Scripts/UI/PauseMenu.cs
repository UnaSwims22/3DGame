using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] string controlsSceneName = "Controls";
    
    public GameObject pauseMenuUI;
    public GameObject resume;
    public GameObject quit;
    public GameObject controls;

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

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
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

    public void PressControlls()
    {
        SceneManager.LoadScene(sceneName: "Controls");
    }

    public void QuitButton()
    {
        SceneManager.LoadScene(sceneName: "Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit pressed");
    }
}
