using System.Collections;
using System.Collections.Generic;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = GetComponent<Camera>();
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

        if (_clueController != null)
        {
            if (Input.GetKeyDown(interactKey))
            {
                _clueController.ShowClue();
            }
        }

    }

    void ClearNote()
    {
        if (_clueController != null)
        {
            HighlightCrosshair(false);
            _clueController = null;
            //Disable crosshair
            //noteController = null;
        }
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



