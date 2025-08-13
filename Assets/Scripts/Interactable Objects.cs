using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    public float interactDistance = 3f;
    public string pickupKey = "e";
    public Text interactText; // Assign in Inspector

    private Color originalColor;
    private Renderer objRenderer;
    private bool isHighlighted = false;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            originalColor = objRenderer.material.color;
        }

        if (interactText != null)
        {
            interactText.enabled = false;
        }
    }

    void Update()
    {
        Transform player = Camera.main.transform;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactDistance)
        {
            HighlightObject(true);

            if (interactText != null)
            {
                interactText.text = $"Press '{pickupKey.ToUpper()}' to pick up";
                interactText.enabled = true;
            }

            if (Input.GetKeyDown(pickupKey))
            {
                PickUp();
            }
        }
        else
        {
            HighlightObject(false);

            if (interactText != null)
            {
                interactText.enabled = false;
            }
        }
    }

    void HighlightObject(bool highlight)
    {
        if (objRenderer != null)
        {
            objRenderer.material.color = highlight ? highlightColor : originalColor;
            isHighlighted = highlight;
        }
    }

    void PickUp()
    {
        // Example: deactivate the object
        gameObject.SetActive(false);

        if (interactText != null)
        {
            interactText.enabled = false;
        }
    }
}
