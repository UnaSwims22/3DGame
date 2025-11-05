using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class EnemyDamageFlash : MonoBehaviour
{
    private List<Renderer> renderers = new List<Renderer>();
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    public Color flashColor = Color.red;
    public float flashDuration = 0.15f;
    

    

    void Start()
    {

        // Fetching all the renderers in enemy model
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (Renderer r in renderers)
        {
            originalColors[r] = r.material.color;
        }

    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {

        foreach (Renderer r in renderers)
            r.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        foreach (Renderer r in renderers)
            r.material.color = originalColors[r];
    }
}


