using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
//using UnityStandardAssets.Characters.FirstPerson;

public class ClueController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode capsKey;

    [Space(10)]
    [SerializeField] private FPController player;

    [Header("UI Text")]
    [SerializeField] private GameObject clueCanvas;
    [SerializeField] private TMP_Text clueTextAreaUI;

    [Space(10)]
    [SerializeField][TextArea] private string clueText;

    [Space(10)]
    [SerializeField] private UnityEvent openEvent;
    private bool isOpen = false;


    public void ShowClue()
    {
        clueTextAreaUI.text = clueText;
        clueCanvas.SetActive(true);
        openEvent.Invoke();
        DisablePlayer(true);
        isOpen = true;
    }

    void DisableClue()
    {
        clueCanvas.SetActive(false);
        clueTextAreaUI.text = null;//may clear
        DisablePlayer(false);
        isOpen = false;
    }

    void DisablePlayer(bool disable)
    {
        player.enabled = !disable;
    }

    private void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                DisableClue();
            }
        }
    }

}


