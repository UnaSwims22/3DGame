using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BatteryPickup : MonoBehaviour
{
    public int batteryAmount = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        AdvancedFlashlight flashlight = other.GetComponentInChildren<AdvancedFlashlight>();
        if (flashlight != null)
        {
            flashlight.AddBattery(batteryAmount);
            Destroy(gameObject);
        }

       
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }
}
