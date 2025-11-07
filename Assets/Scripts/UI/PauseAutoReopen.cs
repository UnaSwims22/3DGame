using UnityEngine;

public class PauseAutoReopen : MonoBehaviour
{
    public PauseMenu pauseMenu;

    void Start()
    {
        if (PauseReturnHelper.ShouldOpenPauseOnLoad)
        {
            PauseReturnHelper.ShouldOpenPauseOnLoad = false;
            pauseMenu.Pause();
        }
    }
}
