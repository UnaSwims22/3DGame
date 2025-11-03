using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public ParticleSystem flashParticles;

    public void PlayFlash()
    {
        if (flashParticles != null)
            flashParticles.Play();
    }
}


