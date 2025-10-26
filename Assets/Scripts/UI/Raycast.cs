using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField] private float rayLength = 5;
    private Camera _camera;

    private ClueController _clueController;

    [Header("Crosshair")]

    [SerializeField] private Image crosshair;

    [Header("Input Key")]
    [SerializeField] private KeyCode interactKey;

    [SerializeField] private TMP_Text interactionHintText;
    [SerializeField] private TMP_Text interactionHintTextDisappear;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, rayLength))
        {
            var readableItem = hit.collider.GetComponent<ClueController>();
            if (readableItem != null)
            {
                _clueController = readableItem;
                HighlightCrosshair(true);

                if (!_clueController.IsOpen)
                {
                    interactionHintText.text = $"Press {interactKey.ToString().ToUpper()} to Read Clue";
                    interactionHintText.enabled = true;

                    if (Input.GetKeyDown(interactKey))
                    {
                        _clueController.ShowClue();
                    }
                }



            }
            else
            {
                ClearNote();

            }
        }
        else
        {
            ClearNote();
        }

        if (_clueController != null && _clueController.IsOpen)
        {
            interactionHintText.text = "Press T to Exit";
            interactionHintText.enabled = true;

            if (Input.GetKeyDown(KeyCode.T))
            {
                // Simulate closing clue (calls DisableClue() via Update in ClueController)
                _clueController.SendMessage("DisableClue", SendMessageOptions.DontRequireReceiver);
                ClearNote();

                if (Input.GetKeyDown(interactKey))
                {
                    _clueController.ShowClue();
                }
            }

        }

        void ClearNote()
        {
            if (_clueController != null && !_clueController.IsOpen)
            {
                HighlightCrosshair(false);
                _clueController = null;
                //Disable crosshair

            }

            //  Hide the hint
            interactionHintTextDisappear.text = "";
            interactionHintTextDisappear.enabled = false;


        }

        void HighlightCrosshair(bool on)
        {
            if (on)
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
            }
        }
    }
}




