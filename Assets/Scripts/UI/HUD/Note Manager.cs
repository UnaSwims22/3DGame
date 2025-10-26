using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance;

    public GameObject notePanel;
    public TextMeshProUGUI noteTitle;
    public TextMeshProUGUI noteContent;
    public Image noteImage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DontDestroyOnLoad(gameObject);
    }

    public void OpenNote(Note note)
    {
        if (notePanel == null)
        {
            Debug.LogError("❌ NotePanel not assigned in NoteManager!");
            return;
        }

        notePanel.SetActive(true);
        noteTitle.text = note.noteTitle;
        noteContent.text = note.noteContent;
        noteImage.sprite = note.noteImage;
    }

    public void CloseNote()
    {
        if (notePanel == null) return;
        notePanel.SetActive(false);
        
    }
}



