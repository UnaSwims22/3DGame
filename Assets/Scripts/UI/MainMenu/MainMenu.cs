using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] string gameSceneName = "Game";
    [SerializeField] string controlsSceneName = "Controls";

    [Header("Animation Objects")]
    public Animator backgroundAnimator;
    public Animator handAnimator;
    public CanvasGroup fadeCanvas;

    private bool isTransitioning = false;
    
    void Update()
    {
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true) Quit();
        if (Gamepad.current?.startButton.wasPressedThisFrame == true) StartGame();
    }

    // Button hooks
    public void StartGame()
    {
        if (!isTransitioning)
            StartCoroutine(StartGameTransition());
    }

    //Called by controlls button
    public void OpenControls()
    {
        SceneManager.LoadScene(controlsSceneName);
    }
        
    //Called by back button in controls scene
    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator StartGameTransition()
    {
        isTransitioning = true;

        //Trigger falling animation
        if (backgroundAnimator) backgroundAnimator.SetTrigger("Fall");
        if (handAnimator) handAnimator.SetTrigger("Fall");

        //Fade to black slowly
        float duration = 2f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            if (fadeCanvas)
                fadeCanvas.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }

        SceneManager.LoadScene(gameSceneName);
    }
   
    
}


