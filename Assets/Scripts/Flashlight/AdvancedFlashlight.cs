using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvancedFlashlight : MonoBehaviour
{

    public Light light;

    public BatteryLifeBar batteryLifeBar;
    public TMP_Text text;

    public TMP_Text batteryText;

    public float lifetime = 100;

    public float batteries = 0;

    public AudioSource flashON;
    public AudioSource flashOFF;
    public AudioSource rechargeSound;

    private bool on;
    private bool off;


    void Start()
    {
        light = GetComponent<Light>();

        off = true;
        light.enabled = false;

    }



    void Update()
    {
        text.text = ("Battery Life     ") + lifetime.ToString("0") + "%";
        batteryText.text = batteries.ToString();

        HandleToggle();
        HandleRecharge();
        HandleDrain();

    }

    void HandleToggle()
    {

        if (Input.GetKeyDown(KeyCode.Keypad1) && off)
        {
            flashON.Play();
            light.enabled = true;
            on = true;
            off = false;
        }

        else if (Input.GetKeyDown(KeyCode.Keypad1) && on)
        {
            flashOFF.Play();
            light.enabled = false;
            on = false;
            off = true;
        }

    }

    void HandleDrain()
    {

        if (on)
        {
            lifetime -= 1 * Time.deltaTime;
        }

        if (lifetime <= 0)
        {
            light.enabled = false;
            on = false;
            off = true;
            lifetime = 0;
        }

        if (lifetime >= 100)
        {
            lifetime = 100;
        }

    }

    void HandleRecharge()
    { 

        if (Input.GetKeyDown(KeyCode.Keypad2) && batteries >= 1)
        {
            batteries -= 1;
            lifetime += 50;
            rechargeSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) && batteries == 0)
        {
            return;
        }

        if (batteries <= 0)
        {
            batteries = 0;
        }



    }
     public void AddBattery(int amount)
    {
        batteries += amount;
    }

}


