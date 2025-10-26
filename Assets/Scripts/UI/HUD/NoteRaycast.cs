using TMPro;
using UnityEngine;

public class NoteRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private LayerMask noteLayer;

    [Header("Input Keys")]
    [SerializeField] private KeyCode interactKey = KeyCode.CapsLock; // CAPS to read
    [SerializeField] private KeyCode exitKey = KeyCode.T;             // T to close

    [Header("Hint UI")]
    [SerializeField] private TextMeshProUGUI hintText;

    private Note currentNote;

    void Update()
    {
        // Cast a ray from the camera forward
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength, noteLayer))
        {
            Note note = hit.collider.GetComponent<Note>();

            if (note != null)
            {
                currentNote = note;
                ShowHint("Press CAPS to read note");

                if (Input.GetKeyDown(interactKey))
                {
                    NewMethod(note);
                    ShowHint("Press T to close note");
                }
            }
            else
            {
                ClearHint();
                currentNote = null;
            }
        }
        else
        {
            ClearHint();
            currentNote = null;
        }

        // Close note logic
        if (NoteManager.Instance != null && NoteManager.Instance.notePanel.activeSelf)
        {
            if (Input.GetKeyDown(exitKey))
            {
                NoteManager.Instance.CloseNote();
                ClearHint();
            }
        }
    }

    private static void NewMethod(Note note)
    {
        NoteManager.Instance.OpenNote(note);
    }

    void ShowHint(string message)
    {
        if (hintText != null)
        {
            hintText.text = message;
            hintText.gameObject.SetActive(true);
        }
    }

    void ClearHint()
    {
        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }
}
