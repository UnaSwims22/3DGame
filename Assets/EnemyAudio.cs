using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hurtSound;


    public void PlayHurt()
    {
        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);
    }
    
}
