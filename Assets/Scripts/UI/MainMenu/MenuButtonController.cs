using UnityEngine;

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
        float vertical = Input.GetAxis("Vertical");

        //Keyboard navigation
        if (vertical != 0)
        {
           if (!keyDown)
           { 
                if (vertical < 0)
                index = (index < maxIndex) ? index + 1 : 0;
            else if (vertical > 0)
                index = (index > 0) ? index - 1 : maxIndex;

            keyDown = true; 
            
           }
           
        }
        else
        {
            keyDown = false;
        }
    }

    public void SetIndexByHover (int newIndex)
    {
        index = newIndex;
    }



}
