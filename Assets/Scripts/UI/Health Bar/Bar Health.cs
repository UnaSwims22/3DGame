using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BarHealth : MonoBehaviour
{
    public TMP_Text healthText; // health text 
    public Image healthBar; //actual ui bar
    public Image[] healthPoints;

    float health, maxHealth = 100f;
    float lerpSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        healthText.text = "Health:" + health + "%";
        if (health > maxHealth) health = maxHealth;

        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();


        if (Input.GetKeyDown(KeyCode.Space))
            Damage(10f);
        if (Input.GetKeyDown(KeyCode.H))
            Heal(10f);
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health / maxHealth), lerpSpeed);
        for (int i = 0; i < healthPoints.Length; i++)
        {
            healthPoints[i].enabled = !DisplayHealthPoint(health, i);
        }
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }

    public void Damage(float damagePoints)
    {
        if (health > 0)

            health -= damagePoints;
    }

    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
            health += healingPoints;

    }


}
