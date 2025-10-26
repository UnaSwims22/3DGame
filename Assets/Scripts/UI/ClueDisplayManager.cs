using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueDisplayManager : MonoBehaviour
{
    public static ClueDisplayManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject cluePanel;
    [SerializeField] private Image clueImage;
    [SerializeField] private TMP_Text clueDescription;
    [SerializeField] private TMP_Text exitHintText; //  “Press Tto exit” text

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("✅ ClueDisplayManager successfully initialized!");
    }

    private void Start()
    {
        // Hide the panel on start
        if (cluePanel != null)
            cluePanel.SetActive(false);
    }

    public void ShowClueUI(Sprite clueSprite, string clueText)
    {
        if (cluePanel == null || clueImage == null || clueDescription == null)
        {
            Debug.LogError(" ClueDisplayManager is missing UI references!");
            return;
        }

        cluePanel.SetActive(true);

        clueImage.sprite = clueSprite;
        clueDescription.text = clueText;

        if (exitHintText != null)
            exitHintText.gameObject.SetActive(true);
    }

    public void HideClueUI()
    {
        if (cluePanel != null)
            cluePanel.SetActive(false);

        if (exitHintText != null)
            exitHintText.gameObject.SetActive(false);
    }
}


    

