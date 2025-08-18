using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DamageFlash : MonoBehaviour
{
    [SerializeField] Image overlay;   
    [SerializeField] float flashDuration = 0.2f;
    [SerializeField] float maxAlpha = 0.5f;

    Coroutine routine;

    public void Flash()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(0f, maxAlpha, t / flashDuration));
            yield return null;
        }
        t = 0f;
        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(maxAlpha, 0f, t / flashDuration));
            yield return null;
        }
        SetAlpha(0f);
        routine = null;
    }

    void SetAlpha(float a)
    {
        if (!overlay) return;
        var c = overlay.color; c.a = a; overlay.color = c;
    }
}


