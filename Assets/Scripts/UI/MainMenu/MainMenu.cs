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

    [Header("Camera Animation Settings")]
    public Camera mainCamera;
    public float zoomTarget = 35f;
    public float zoomSpeed = 10f;
    public Vector3 cameraMoveTarget = new Vector3(0, -1f, -5f);
    public float moveSpeed = 2f;

    private bool isTransitioning = false;
    private float originalFOV;
    private Vector3 originalCamPos;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalFOV = mainCamera.fieldOfView;
        originalCamPos = mainCamera.transform.position;
    }


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

    public void PressPlay()
    {
        SceneManager.LoadScene(gameSceneName);
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

            // Camera zoom-in and move-down effect
            mainCamera.fieldOfView = Mathf.Lerp(originalFOV, zoomTarget, t / duration);
            mainCamera.transform.position = Vector3.Lerp(originalCamPos, cameraMoveTarget, t / duration);


            if (fadeCanvas)
                fadeCanvas.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }

        SceneManager.LoadScene(gameSceneName);
    }
   
    
}


