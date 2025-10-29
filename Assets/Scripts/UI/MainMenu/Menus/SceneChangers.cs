using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangers : MonoBehaviour
{
    
    public void ToGamepadMenu()
    {
        SceneManager.LoadScene("Gamepad");
    }

    public void ReturnToKeyBoard()
    {
        SceneManager.LoadScene("Controls");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
   
}
