using UnityEngine;

public class SceneFlowManager : MonoBehaviour
{
    public enum Source
    {
        None,
        MainMenu,
        PauseMenu
    }

    // Last place player comes from when entering Controls
    public static Source lastSource = Source.None;

    
}
