using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] string controlsSceneName = "Controls";
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string gameplaySceneName = "Game";

    [Header("UI")]
    public GameObject pauseMenuUI;

    [Header("Game Over Settings")]
    public GameObject gameOverUI;
    public bool isGameOver = false;

    [Header("Button Settings")]
    public Button[] buttons;         //all buttons assigned here
    public Color normalColor = Color.white; //unselected
    public Color selectedColor = Color.grey;  //selected
    public float responseSpeed = 500f;


    private RectTransform[] buttonRects;
    private bool isPaused = false;

    public int index = 0;
    private float cooldownTimer = 0.0f;

    void Start()
    {
        pauseMenuUI.SetActive(false);

        buttonRects = new RectTransform[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
            buttonRects[i] = buttons[i].GetComponent<RectTransform>();

        UpdateButtonVisuals();
        
    }

    void Update()
    {
        // PAUSE TOGGLE
        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) 
                Resume();
            else 
                Pause();
        }

        if (!isPaused && !isGameOver) return;

        cooldownTimer -= Time.unscaledDeltaTime;

        HandleInput();



        if (pauseMenuUI != null) pauseMenuUI.SetActive(isPaused);
        if (gameOverUI != null) gameOverUI.SetActive(isGameOver);

    }
    private void HandleInput()
    {
        if (cooldownTimer > 0f) return;

        bool changed = false;

        // DOWN
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index = (index + 1) % buttons.Length;
            changed = true;
        }

        // UP
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index--;
            if (index < 0) index = buttons.Length - 1;
            changed = true;
        }

        // ENTER = activate selected button
        if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[index].onClick.Invoke();
            return;
            
        }

        if (changed)
        {
            cooldownTimer = 1f / responseSpeed; 
            UpdateButtonVisuals();
        }
    }

    private void UpdateButtonVisuals()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i == index) ? selectedColor : normalColor;
            buttons[i].colors = colors;
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  //freezes game
        isPaused = true;

        index = 0;
        UpdateButtonVisuals();
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  //unfreezes game
        isPaused = false;

    }

    public void OnControlsPressed()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName: "Controls");
    }

    public void OnQuitToMainPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName: "MainMenu");
    }

    public void OnQuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Debug.Log("Quit pressed");
    }
}
