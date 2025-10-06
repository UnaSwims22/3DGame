using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightPulse : MonoBehaviour
{

    [Header("Pulse Settings")]
    public float minIntensity = 3f;     // Minimum light intensity
    public float maxIntensity = 5f;     // Maximum light intensity
    public float pulseSpeed = 2f;       // How fast the light pulses
    public bool randomizeStart = true;  // Randomize pulse phase per object

    private Light targetLight;
    private float offset;

    void Start()
    {
        targetLight = GetComponent<Light>();

        // Random start so multiple lights don't sync
        offset = randomizeStart ? Random.Range(0f, Mathf.PI * 2f) : 0f;
    }

    void Update()
    {
        if (targetLight == null) return;

        // Sine wave for smoothly pulse intensity
        float t = (Mathf.Sin(Time.time * pulseSpeed + offset) + 1f) * 0.5f; // 0 → 1
        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}


