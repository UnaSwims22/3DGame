using UnityEngine;

public class Outline : MonoBehaviour
{
    private Outline outlineComp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        outlineComp =GetComponent<Outline>();
    }

    // Update is called once per frame
    private void OnMouseEnter()
    {
        if (outlineComp != null)
        {
            outlineComp.enabled = true;
        }
    }
    private void OnMouseExit()
    {
        if (outlineComp != null)
        {
            outlineComp.enabled = false;
        }
    }


}
