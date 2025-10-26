using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BarHealth : MonoBehaviour
{
    public TMP_Text healthText; // health text 
    public Image healthBar; //actual ui bar
    public Image[] healthPoints;

    float currentHealth, maxHealth = 100f;
    float lerpSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        healthText.text = "Health:" + currentHealth + "%";
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();


        
    }

     public void SetHealth(float current, float max)
    {
        currentHealth = current;
        maxHealth = max;
        healthText.text = "Health: " + Mathf.RoundToInt((currentHealth / maxHealth) * 100) + "%";
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (currentHealth / maxHealth), lerpSpeed);
        for (int i = 0; i < healthPoints.Length; i++)
        {
            healthPoints[i].enabled = !DisplayHealthPoint(currentHealth, i);
        }
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }

    public void Damage(float damagePoints)
    {
        if (currentHealth > 0)

            currentHealth -= damagePoints;
    }

    public void Heal(float healingPoints)
    {
        if (currentHealth < maxHealth)
            currentHealth += healingPoints;

    }


}
