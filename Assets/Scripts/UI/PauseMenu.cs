using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PauseMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] string controlsSceneName = "Controls";
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string gameplaySceneName = "Game";

    [Header("UI")]
    private GameObject pauseMenuUI;
    public Button[] buttons;
    

    [Header("Visual Settings")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.grey;
    public float responseSpeed = 500f;

    private RectTransform[] buttonRects;
    private bool isPaused = false;

    private int index = 0;
    private float cooldownTimer = 0.0f;
    private float inputCooldown => (1f / responseSpeed);


    void Start()
    {
        pauseMenuUI.SetActive(false);

        buttonRects = new RectTransform[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonRects[i] = buttons[i].GetComponent<RectTransform>();
        }
    }
    void Update()
    {
        // PAUSE TOGGLE
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }

        if (!isPaused) return;

        cooldownTimer -= Time.unscaledDeltaTime;

        HandleInput();
        
    } 
    private void HandleInput()
    {
        if (cooldownTimer > 0f) return;

        bool changed = false;

        // DOWN
        if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
        {
            index = (index + 1) % buttons.Length;
            changed = true;
        }

        // UP
        else if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
        {
            index--;
            if (index < 0) index = buttons.Length - 1;
            changed = true;
        }

        // ENTER = activate selected button
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
        {
            buttons[index].onClick.Invoke();
            changed = true;
        }

        if (changed)
        {
            cooldownTimer = inputCooldown;
            UpdateButtonVisuals();
        }
    }
    private void UpdateButtonVisuals()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock c = buttons[i].colors;
            c.normalColor = (i == index) ? selectedColor : normalColor;
            buttons[i].colors = c;
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
        PauseReturnHelper.ShouldOpenPauseOnLoad = true;
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
