using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] string gameSceneName = "Game";

    // Button hooks
    public void Play() => SceneManager.LoadScene(gameSceneName);

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

   
    void Update()
    {
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true) Quit();
        if (Gamepad.current?.startButton.wasPressedThisFrame == true) Play();
    }
}


