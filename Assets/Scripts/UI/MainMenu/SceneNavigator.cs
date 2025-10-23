using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 1f; // match fade animation length

    public void LoadScene(string sceneName)
    {
        StartCoroutine(PlayTransition(sceneName));
    }

    private System.Collections.IEnumerator PlayTransition(string sceneName)
    {
        // Trigger fade-out animation
        transitionAnimator.SetTrigger("FadeOut");

        // Wait for animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }
  
}
