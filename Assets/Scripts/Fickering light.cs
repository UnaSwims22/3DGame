using System.Runtime.CompilerServices;
using UnityEngine;

public class Fickeringlight : MonoBehaviour
{
    private Light light;

    public float minIntensity = .5f;
    public float maxIntensity = 3.0f;
    public float flickerSpeed = 5.1f;

    private void Start()
    {
        light = GetComponent<Light>();

        InvokeRepeating("Flicker", 0f, flickerSpeed);

    }
    private void Flicker()
    {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        light.intensity = randomIntensity;
    }

}
