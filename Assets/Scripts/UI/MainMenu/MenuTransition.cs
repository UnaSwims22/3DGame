using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MenuTransition : MonoBehaviour
{
    [Header("Scene Names")]
    public string gameSceneName = "Game";
    public string controlsSceneName = "Controls";

    [Header("References")]
    public Animator bgAnimator;
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;
    public Animator fadeAnimator;
    public AudioSource musicSource;
    public AudioClip menuMusic;

    [Header("Timing")]
    public float transitionDuration = 1.2f;

    private bool isTransitioning = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (musicSource && !musicSource.isPlaying)
        {
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.Play();
            DontDestroyOnLoad(musicSource.gameObject);
        }
    }

    public void OnPlayPressed()
    {
        if (!isTransitioning)
            StartCoroutine(TransitionAndLoad(gameSceneName));
    }

    public void OnControlsPressed()
    {
        if(!isTransitioning)
            StartCoroutine(TransitionAndLoad(controlsSceneName));
        
    }

    public IEnumerator TransitionAndLoad(string sceneName)
    {
        isTransitioning = true;

        // Play animations
        if (bgAnimator) bgAnimator.SetTrigger("Fall");
        if (leftHandAnimator) leftHandAnimator.SetTrigger("Fall");
        if (rightHandAnimator) rightHandAnimator.SetTrigger("Fall");
        if (fadeAnimator) fadeAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(sceneName);
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
