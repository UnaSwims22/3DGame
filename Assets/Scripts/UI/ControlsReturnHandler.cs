using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsReturnHandler : MonoBehaviour
{
    [SerializeField] private string pauseMenuSceneName = "Game";
    [SerializeField] private string mainMenuSceneName = "Main Menu";

    public void ReturnButton()
    {
        if (PauseMenu.comingFromPauseMenu)
        {
            PauseMenu.comingFromPauseMenu = false;
            SceneManager.LoadScene(pauseMenuSceneName);

            // After loading back into the gameplay scene,
            
        }
        else
        {
            // coming from from Main Menu
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}


