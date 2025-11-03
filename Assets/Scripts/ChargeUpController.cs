using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class ChargeUpController : MonoBehaviour
{
    [Header("Charge Settings")]
    public float chargeTime = 0.8f;
    public AnimationCurve glowCurve;

    [Header("Glow Object")]
    public Transform gunGlowObject;
    public float glowScale = 0.4f;

    [Header("VFX")]
    public ParticleSystem chargeParticles;
    public ParticleSystem overchargeParticles;

    [Header("Bloom")]
    public Volume postProcessVolume;
    private Bloom bloom;

    private float chargeTimer = 0f;
    private bool charging = false;

    void Start()
    {
        if (postProcessVolume != null)
            postProcessVolume.profile.TryGet(out bloom);

        if (gunGlowObject != null)
            gunGlowObject.localScale = Vector3.zero;

        if (chargeParticles != null)
            chargeParticles.Stop();
        if (overchargeParticles != null)
            overchargeParticles.Stop();
    }

    public void BeginCharge()
    {
        charging = true;
        chargeTimer = 0f;

        if (chargeParticles != null)
            chargeParticles.Play();
    }

    public bool ReleaseCharge()
    {
        bool fullyCharged = chargeTimer >= chargeTime;

        charging = false;

        if (chargeParticles != null)
            chargeParticles.Stop();

        if (overchargeParticles != null)
            overchargeParticles.Stop();

        // reset glow
        if (gunGlowObject != null)
            gunGlowObject.localScale = Vector3.zero;

        // reset bloom
        if (bloom != null)
            bloom.intensity.Override(0.5f);

        return fullyCharged;
    }

    void Update()
    {
        if (!charging) return;

        chargeTimer += Time.deltaTime;

        float t = Mathf.Clamp01(chargeTimer / chargeTime);
        float glow = glowCurve.Evaluate(t);

        if (gunGlowObject != null)
            gunGlowObject.localScale = Vector3.one * glow * glowScale;

        if (bloom != null)
            bloom.intensity.Override(3f * glow);

        // Overcharge FX
        if (t >= 1f && overchargeParticles != null && !overchargeParticles.isPlaying)
            overchargeParticles.Play();
    }

    public float ChargePercent()
    {
        return Mathf.Clamp01(chargeTimer / chargeTime);
    }

}
  