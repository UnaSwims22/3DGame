using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    
    
    public MenuNavigator navigator;
    [SerializeField] int thisIndex;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuButtonController.SetIndexByHover(thisIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("selected", false);
    }
  
    public void OnPointerClick(PointerEventData eventData)
    {
        animator.SetBool("pressed", true);
    }

 
    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);

            if (Input.GetButtonDown("Submit"))
            {
                animator.SetBool("pressed", true);
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }

    }
}
