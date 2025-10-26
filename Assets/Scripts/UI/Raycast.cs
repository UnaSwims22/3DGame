using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float rayLength = 5f;
    private Camera _camera;

    [Header("UI References")]
    [SerializeField] private Image crosshair;
    [SerializeField] private TMP_Text interactionHintText;

    [Header("Keys")]
    [SerializeField] private KeyCode interactKey = KeyCode.CapsLock;
    [SerializeField] private KeyCode exitKey = KeyCode.T;

    private ClueController _clueController;

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // If player is viewing a clue, handle exit input
        if (_clueController != null && _clueController.IsOpen)
        {
            interactionHintText.text = $"Press {exitKey} to Exit";
            interactionHintText.enabled = true;

            if (Input.GetKeyDown(exitKey))
            {
                _clueController.CloseClue();
                ClearHint();
            }
            return;
        }

        // Otherwise, handle normal clue detection
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
        {
            var clue = hit.collider.GetComponent<ClueController>();

            if (clue != null)
            {
                _clueController = clue;
                HighlightCrosshair(true);

                interactionHintText.text = $"Press {interactKey.ToString().ToUpper()} to Read Clue";
                interactionHintText.enabled = true;

                if (Input.GetKeyDown(interactKey))
                {
                    _clueController.ShowClue();
                }
            }
            else
            {
                ClearHint();
            }
        }
        else
        {
            ClearHint();
        }
    }

    void ClearHint()
    {
        HighlightCrosshair(false);
        interactionHintText.text = "";
        interactionHintText.enabled = false;
        _clueController = null;
    }

    void HighlightCrosshair(bool on)
    {
        crosshair.color = on ? Color.red : Color.white;
    }
}




