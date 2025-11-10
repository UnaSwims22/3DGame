using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }// Start is called once before the first execution of Update after the MonoBehaviour is created
  
}
