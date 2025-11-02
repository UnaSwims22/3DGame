using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BatteryLifeBar : MonoBehaviour
{
    public TMP_Text batteryHealthText; // life line text, attach to "Batter Lifeline gameObject"
    public Image batteryBar; //actual BATTERY bar
    public Image[] barLifePoints;

    public float currentBatterylifetime, maxBatteryLifetime  = 100f;
    float lerpSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBatterylifetime = maxBatteryLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        batteryHealthText.text = ("Battery Life     ") + maxBatteryLifetime.ToString("0") + "%";
        
        if (currentBatterylifetime > maxBatteryLifetime) currentBatterylifetime = maxBatteryLifetime;

        lerpSpeed = 3f * Time.deltaTime;
        BatteryBarFiller();
    }

    public void BatteryBarFiller ()
    {
        batteryBar.fillAmount = Mathf.Lerp(batteryBar.fillAmount, (currentBatterylifetime / maxBatteryLifetime), lerpSpeed);
        for (int i = 0; i < barLifePoints.Length; i++)
        {
            barLifePoints[i].enabled = !DisplayBatteryLifePoint(currentBatterylifetime, i);
        }
    }


    bool DisplayBatteryLifePoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }

    public void Damage(float damagePoints)
    {
        if (currentBatterylifetime > 0)

            currentBatterylifetime -= damagePoints;
    }

    public void Recharge(float healingPoints)
    {
        if (currentBatterylifetime < maxBatteryLifetime)
            currentBatterylifetime += healingPoints;

    }


}


