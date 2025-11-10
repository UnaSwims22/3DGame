using UnityEngine;

public class DorianTrigger : MonoBehaviour
{
    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell the timer script the player found Dorian
            FindObjectOfType<GameTimer>().PlayerFoundPerson();
        }
    }
}
