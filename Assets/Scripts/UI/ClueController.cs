using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ClueController : MonoBehaviour
{
    [Header("Clue Content")]
    [SerializeField] private Sprite clueSprite;         // unique image for this clue
    [SerializeField][TextArea] private string clueText; // unique description text

    [Header("Player Control")]
    [SerializeField] private FPController player;       // reference to your player controller

    [Header("Events (optional)")]
    [SerializeField] private UnityEvent onClueOpened;
    [SerializeField] private UnityEvent onClueClosed;

    private bool isOpen = false;
    public bool IsOpen => isOpen;

    
        public void ShowClue()
    {
        Debug.Log("Trying to show clue for " + gameObject.name);

        if (ClueDisplayManager.Instance == null)
        {
            Debug.LogError("❌ ClueDisplayManager.Instance is NULL!");
            return;
        }

        ClueDisplayManager.Instance.ShowClueUI(clueSprite, clueText);
        DisablePlayer(true);
        isOpen = true;
    }

        
    

    public void CloseClue()
    {
        if (!isOpen) return;

        isOpen = false;
        onClueClosed?.Invoke();

        ClueDisplayManager.Instance.HideClueUI();
        DisablePlayer(false);
    }

    private void DisablePlayer(bool disable)
    {
        if (player != null)
            player.enabled = !disable;
    }

    private void Update()
    {
        // Press "T" to close the clue
        if (isOpen && Input.GetKeyDown(KeyCode.T))
        {
            CloseClue();
        }
    }
}




