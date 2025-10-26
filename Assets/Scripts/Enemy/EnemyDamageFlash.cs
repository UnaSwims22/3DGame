using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class EnemyDamageFlash : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    private Coroutine flashRoutine;

    public Color flashColor = Color.red;
    public float flashDuration = 0.15f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        if (rend == null) yield break;

        rend.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
    }
}


