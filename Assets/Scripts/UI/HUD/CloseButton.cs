using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(CloseNote);
    }

    private void CloseNote()
    {
        NoteManager.Instance.CloseNote();
    }
}


