using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] string controlsSceneName = "Controls";
    [SerializeField] string mainMenuSceneName = "Main Menu";
    
    public GameObject pauseMenuUI;
    public GameObject resume;
    public GameObject quit;
    public GameObject controls;

    private bool isPaused = false;

    public static bool comingFromPauseMenu = false;

     void Start()
    {
        if (comingFromPauseMenu)
        {
            comingFromPauseMenu = false;
            Pause();
        }
    }
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
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  //unfreezes game
        isPaused = false;
    }

    public void PressControls()
    {
        comingFromPauseMenu = true;

        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName: "Controls");
    }

    public void QuitToMainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName: "Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit pressed");
    }
}
