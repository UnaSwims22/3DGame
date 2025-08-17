using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class SceneMngr : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public Button optionsButton;
    public Button exitButton;
    public GameObject menu;
    public FPController playerController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Prevent player movement when paused
        if (menu.activeSelf)
        {
            // Optionally, you can disable the player controller script here
            // Example: playerController.enabled = false;
            // Or set a static flag for movement scripts to check
        }

        // Listen for Escape key to toggle pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menu.activeSelf)
            {
                // Pause and show menu
                menu.SetActive(true);
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                // Disable player movement here
                playerController.enabled = false;
            }
            else
            {
                // Resume and hide menu
                menu.SetActive(false);
                Time.timeScale = 1f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                // Enable player movement here
                playerController.enabled = true;
            }
        }

        // Listen for Enter key to resume if menu is active
        if (menu.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            menu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            // Enable player movement here
            playerController.enabled = true;
        }
    }
    public void OnStartButtonClicked()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Unarine");
    }
    public void OnOptionsButtonClicked()
    {
        //this will Open our game options menu
        Debug.Log("Options button clicked");
        
    }
    public void OnExitButtonClicked()
    {
        // Exit the application
        Application.Quit();
        Debug.Log("Exit button clicked");
    }
    public void Clicked()
    {
        // Toggle the menu visibility
        menu.SetActive(!menu.activeSelf);
        if (menu.activeSelf)
        {
            // Pause the game when the menu is open
            Time.timeScale = 0f;
        }
        else
        {
            // Resume the game when the menu is closed
            Time.timeScale = 1f;
        }
    }
    public void OnResumeButtonClicked()
    {
        // Resume the game and hide the menu
        menu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OnMainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
