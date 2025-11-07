using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TMP_Text textOB;
    public GameObject Activator;
    public string dialogue = "Dialogue";

    public float timer = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textOB.GetComponent<TMP_Text>().enabled = false;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            textOB.GetComponent<TMP_Text>().enabled = true;
            textOB.text = dialogue.ToString();
            StartCoroutine(DisableText());
        }
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(timer);
        textOB.GetComponent<TMP_Text>().enabled = false;
        Destroy(Activator);
    }
}
