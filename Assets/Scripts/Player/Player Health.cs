using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI References")]
    public BarHealth healthBar;
    public Image damageIndicator; // Vignette overlay image

    [Header("Damage Indicator Settings")]
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f); // flash color when hit
    public float flashSpeed = 5f;
    public float lowHealthThreshold = 30f;
    public float pulseSpeed = 3f; // pulse speed for vignette when low HP

    [Header("Camera Shake Settings")]
    public Camera mainCamera;
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.2f;

    [Header("Audio Settings")]
    public AudioClip heartbeatClip; // assign a slow, bassy heartbeat sound
    public float heartbeatInterval = 1f; // delay between beats at low health
    private AudioSource damageAudio;

    private bool isFlashing = false;
    private bool isPulsing = false;
    private Coroutine pulseRoutine;
    private Coroutine heartbeatRoutine;
    private Coroutine damageFlashCoroutine;
    private Color baseColor;
    private bool isLowHealthActive = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (damageIndicator != null)
        {
            baseColor = damageIndicator.color;
            damageIndicator.color = Color.clear;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        damageAudio = GetComponent<AudioSource>();
        damageAudio.loop = false;
        damageAudio.playOnAwake = false;

        
    }

    // Update is called once per frame
    private void Update()
    {
        // Test damage (press K)
        if (Input.GetKeyDown(KeyCode.J))
            TakeDamage(20f);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        

        if (damageAudio)
            damageAudio.Play();

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        // Flash red and shake camera
        if (damageIndicator != null && !isFlashing)
            StartCoroutine(DamageFlash());
        if (mainCamera != null)
            StartCoroutine(ScreenShake());

        // Handle low-health feedback
        if (currentHealth <= lowHealthThreshold)
        {
            if (!isPulsing)
                pulseRoutine = StartCoroutine(LowHealthPulse());
            if (heartbeatRoutine == null)
                heartbeatRoutine = StartCoroutine(HeartbeatLoop());
        }
        else
        {
            StopLowHealthEffects();
        }

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        // Stop pulse & heartbeat when healed above threshold
        if (currentHealth > lowHealthThreshold)
            StopLowHealthEffects();
    }

    private void StopLowHealthEffects()
    {
        if (isPulsing && pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
           
            isPulsing = false;
        }

        

        if (heartbeatRoutine != null)
        {
            StopCoroutine(heartbeatRoutine);
            heartbeatRoutine = null;
        }

        if (damageAudio.isPlaying)
            damageAudio.Stop();
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator DamageFlash()
    {
        isFlashing = true;

        if (damageIndicator == null)
        {
            Debug.LogWarning("Damage Indicator not assigned");
            yield break;

        }

        damageIndicator.color = flashColor;

        float timer = 0f;
        float flashDuration = 0.3f; // short visible flash

        // Hold the red flash for a brief moment
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        while (damageIndicator.color.a > 0.05f)
        {
            damageIndicator.color = Color.Lerp(damageIndicator.color, Color.clear, flashSpeed * Time.deltaTime);
            yield return null;
        }

        damageIndicator.color = Color.clear;
        isFlashing = false;
    }

    IEnumerator LowHealthPulse()
    {
        isPulsing = true;
        float t = 0;

        while (currentHealth <= lowHealthThreshold)
        {
            t += Time.deltaTime * pulseSpeed;
            float alpha = Mathf.PingPong(t, 0.4f); // smooth pulsing opacity
            
            yield return null;
        }

        damageIndicator.color = Color.clear;
        isPulsing = false;
    }

    IEnumerator HeartbeatLoop()
    {
        while (currentHealth <= lowHealthThreshold)
        {
            if (heartbeatClip != null && !damageAudio.isPlaying)
            {
                damageAudio.PlayOneShot(heartbeatClip);
            }

            // heartbeat speeds up slightly as health gets lower
            float speedMultiplier = Mathf.Lerp(1f, 0.5f, currentHealth / lowHealthThreshold);
            yield return new WaitForSeconds(heartbeatInterval * speedMultiplier);
        }
    }

    IEnumerator ScreenShake()
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPickup"))
        {
            HealthPickup pickup = other.GetComponent<HealthPickup>();
            if (pickup != null)
            {
                Heal(pickup.amount);
                Destroy(other.gameObject);
            }
        }

    }

}


[System.Serializable]
public class HealthPickup : MonoBehaviour
{
    public float amount = 50f;
}
