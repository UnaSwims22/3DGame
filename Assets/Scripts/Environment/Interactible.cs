using UnityEngine;

public class InteractableGlow : MonoBehaviour
{
    public Renderer objectRenderer;
    public Material normalMaterial;
    public Material glowMaterial;

    private bool isGlowing = false;

    void Update()
    {
        if (isGlowing && objectRenderer != null)
        {
            float pulse = (Mathf.Sin(Time.time * 4f) + 1f) / 2f;
            Color baseColor = glowMaterial.GetColor("_EmissionColor");
            Color pulsingColor = baseColor * Mathf.LinearToGammaSpace(Mathf.Lerp(1f, 3f, pulse));
            objectRenderer.material.SetColor("_EmissionColor", pulsingColor);
        }
          

        SetGlow(false);
    }

    public void SetGlow(bool state)
    {
        if (objectRenderer != null)
        {
            isGlowing = state;
            objectRenderer.material = state ? glowMaterial : normalMaterial;
        }
    }
}
