using JetBrains.Annotations;
using UnityEngine;

public class menu : MonoBehaviour

{
    public GameObject InGamemenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InGamemenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InGamemenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            InGamemenu.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }
}
