using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class ClueUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject cluePanel;         // The panel that pops up
    public TMP_Text clueDescriptionText; // TextMeshPro text for clue description
    public TMP_Text cluePromptText;      // "Press E to investigate"

    private bool isClueOpen = false;
    private FPController playerMovement;

    private void Start()
    {
        cluePanel.SetActive(false);
        cluePromptText.gameObject.SetActive(false);

        
        playerMovement = ObjectFindAnyObjectByType<FPController>();
    }

    private T ObjectFindAnyObjectByType<T>()
    {
        throw new NotImplementedException();
    }

    public void ShowPrompt(bool state)
    {
        // Only show prompt if no clue is currently open
        if (!isClueOpen)
            cluePromptText.gameObject.SetActive(state);
    }

    public void ShowClue(string clueText)
    {
        isClueOpen = true;
        cluePanel.SetActive(true);
        cluePromptText.gameObject.SetActive(false);
        clueDescriptionText.text = clueText;

        // Lock player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Unlock cursor for UI interaction (optional)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseClue()
    {
        isClueOpen = false;
        cluePanel.SetActive(false);

        // Unlock player movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Re-lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}


