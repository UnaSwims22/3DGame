using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PickupHealth : MonoBehaviour
{
    public float amount = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth currentHealth = other.GetComponent<PlayerHealth>();
        if (currentHealth)
        {
            currentHealth.Heal(amount);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
