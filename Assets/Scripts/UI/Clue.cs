using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Clue : MonoBehaviour
{
    [Header("Clue Settings")]
    public string clueDescription = "This is a mysterious clue."; // Text shown to player
    public float interactionDistance = 3f; 
    public KeyCode interactionKey = KeyCode.T; // Key to press for interaction

    [Header("UI References")]
    public GameObject cluePanel;
    public TMP_Text clueTextUI; // Assign a UI Text element in the Inspector
    public GameObject interactionPrompt;


    private Transform player; // Player's position
    private bool playerInRange = false; // Checks if player is close enough
    private bool clueVisible = false;

    void Start()
    {
      
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

     
        if (cluePanel != null)
            cluePanel.SetActive(false);
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
    }

    void Update()
    {
        if (player == null) return;

        // Check distance between player and clue
        float distance = Vector3.Distance(player.position, transform.position);
        playerInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(playerInRange && !clueVisible);

        // Listen for input only if in range
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            if (!clueVisible)
                ShowClue();
            else
                HideClue();
        }
            
    }

    
    private void ShowClue()
    {
        clueVisible = true;
        if (cluePanel != null && clueTextUI != null)
        {
            clueTextUI.text = clueDescription;
            cluePanel.SetActive(true);

            // Start fade-in effect
            StartCoroutine(FadeCanvasGroup(cluePanel.GetComponent<CanvasGroup>(), 0f, 1f, 0.3f));
        }
    }

   
    private void HideClue()
    {
        clueVisible = false;
        if (cluePanel != null)
        {
            // Fade out then deactivate
            StartCoroutine(FadeOutAndDeactivate());
        }
    }

    private IEnumerator FadeOutAndDeactivate()
    {
        CanvasGroup cg = cluePanel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(cg, 1f, 0f, 0.3f));
        }
        cluePanel.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cg.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}

