using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clue : MonoBehaviour
{
    [Header("Clue Settings")]
    public string clueDescription = "This is a mysterious clue."; // Text shown to player
    public float interactionDistance = 3f; 
    public KeyCode interactionKey = KeyCode.E; // Key to press for interaction

    [Header("UI References")]
    public TMP_Text clueTextUI; // Assign a UI Text element in the Inspector

    private Transform player; // Player's position
    private bool playerInRange = false; // Checks if player is close enough

    void Start()
    {
      
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Hide clue text at start
        if (clueTextUI != null)
            clueTextUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        // Check distance between player and clue
        float distance = Vector3.Distance(player.position, transform.position);
        playerInRange = distance <= interactionDistance;

        
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            ShowClue();
        }
    }

    // Function to display the clue
    private void ShowClue()
    {
        if (clueTextUI != null)
        {
            clueTextUI.text = clueDescription; // Set the UI text
            clueTextUI.gameObject.SetActive(true); // Make it visible

            
            Invoke(nameof(HideClue), 5f); 
        }
    }

   
    private void HideClue()
    {
        if (clueTextUI != null)
            clueTextUI.gameObject.SetActive(false);
    }

   
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}

