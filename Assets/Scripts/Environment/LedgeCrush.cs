using UnityEngine;
using System.Collections;

public class LedgeCrush : MonoBehaviour
{
    [Header("Effects")]
    public AudioClip impactSound;
    public GameObject dustEffectPrefab;
    public float impactForceThreshold = 5f;  

    private AudioSource audioSource;
    private Rigidbody rb;
    private bool hasPlayedEffect = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only trigger heavy impact effects once
        if (hasPlayedEffect) return;

        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce > impactForceThreshold)
        {
            hasPlayedEffect = true;

            // Play sound
            if (audioSource != null && impactSound != null)
                audioSource.PlayOneShot(impactSound);

            // Spawn dust
            if (dustEffectPrefab != null)
                Instantiate(dustEffectPrefab, collision.contacts[0].point, Quaternion.identity);

            // camera shake
            CameraShake shake = Camera.main?.GetComponent<CameraShake>();
            if (shake != null)
                shake.ShakeCamera(0.2f, 0.3f); // duration, intensity
        }
    }
}

