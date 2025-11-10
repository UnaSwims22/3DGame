using UnityEngine;

using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public Text text;
    public GameObject Activator;
    public string dialogue = "Dialogue";

    public float timer = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.GetComponent<Text>().enabled = false;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.GetComponent<Text>().enabled = true;
            text.text = dialogue.ToString();
            StartCoroutine(DisableText());
        }
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(timer);
        text.GetComponent<Text>().enabled = false;
        Destroy(Activator);
    }
}
