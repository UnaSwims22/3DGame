using UnityEngine;
using UnityEngine.InputSystem;

public class MenuButtonController : MonoBehaviour
{
    public int index = 0;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index = (index < maxIndex) ? index + 1 : 0;
            PlayNavigateSound();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index = (index > 0) ? index - 1 : maxIndex;
            PlayNavigateSound();
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.down.wasPressedThisFrame)
                index = (index < maxIndex) ? index + 1 : 0;
            else if (Gamepad.current.dpad.up.wasPressedThisFrame)
                index = (index > 0) ? index - 1 : maxIndex;
        }


    }

    void PlayNavigateSound()
    {
        if (audioSource)
            audioSource.Play();
    }
         
    public void SetIndexByHover (int newIndex)
    {
        index = newIndex;
    }
}
