using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Timer Settings")]
    public float startTime = 480f; // 8 minutes (in seconds)
    private float currentTime;

    [Header("UI Display (Optional)")]
    public Text uiText; // for legacy UI Text
    public TMP_Text tmpText; // for TextMeshPro

    [Header("Scene Names")]
    public string winSceneName = "CongratulationsScene";
    public string loseSceneName = "GameOverScene";

    private bool gameEnded = false;
    private bool playerFoundPerson = false;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime;

        // Clamp to zero
        if (currentTime < 0)
            currentTime = 0;

        // Update timer display
        DisplayTime(currentTime);

        // Check lose condition
        if (currentTime <= 0 && !playerFoundPerson)
        {
            EndGame(false);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (uiText != null)
            uiText.text = formattedTime;

        if (tmpText != null)
            tmpText.text = formattedTime;
    }

    // Call this method when the player finds the person
    public void PlayerFoundPerson()
    {
        if (gameEnded) return;
        playerFoundPerson = true;
        EndGame(true);
    }

    void EndGame(bool won)
    {
        gameEnded = true;
        string sceneToLoad = won ? winSceneName : loseSceneName;
        SceneManager.LoadScene(sceneToLoad);
    }
}
