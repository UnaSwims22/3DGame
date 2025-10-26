using UnityEngine;
using System.Collections;

public class RayShooter : MonoBehaviour
{
    [Header("RayGun References")]
    public Camera _camera;
    public Transform gunTransform;
    public Transform firePoint;
    public GameObject fireballPrefab;

    [Header("Beam Settings")]
    public float projectileSpeed = 30f;
    public float beamWidth = 0.05f;
    public Color beamColor = Color.red;
    public float travelTime = 0.2f;
    public float beamHoldTime = 0.05f;
    public float beamRange = 100f;
    public float beamForce = 300f;

    [Header("Projectile Appearance")]
    public Material projectileMaterial;

    void Start()
    {
        if (_camera == null)
            _camera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            FireRay();
    }

    void FireRay()
    {
        Vector3 screenCenter = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray cameraRay = _camera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, beamRange))
        {
            StartCoroutine(FireVisuals(hitInfo.point));
            HandleHit(hitInfo);
        }
        else
        {
            Vector3 farPoint = cameraRay.GetPoint(beamRange);
            StartCoroutine(FireVisuals(farPoint));
        }

        // Fire projectile
        if (fireballPrefab != null)
        {
            GameObject proj = Instantiate(fireballPrefab, firePoint.position, Quaternion.LookRotation(cameraRay.direction));
            if (projectileMaterial != null)
                proj.GetComponent<Renderer>().material = projectileMaterial;

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = cameraRay.direction * projectileSpeed;

            Destroy(proj, 3f); // Destroy projectile after 3 seconds
        }
    }

    private IEnumerator FireVisuals(Vector3 targetPos)
    {
        // Create a line for beam
        GameObject beamObj = new GameObject("LaserBeam");
        LineRenderer lr = beamObj.AddComponent<LineRenderer>();
        lr.startWidth = beamWidth;
        lr.endWidth = beamWidth;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = beamColor;

        Vector3 startPos = firePoint.position;
        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / travelTime);
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, Vector3.Lerp(startPos, targetPos, t));
            yield return null;
        }

        // Hold beam at full length
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, targetPos);
        yield return new WaitForSeconds(beamHoldTime);

        Destroy(beamObj);

        // Optional: show small impact sphere at hit point
        GameObject impact = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        impact.transform.position = targetPos;
        if (projectileMaterial != null)
            impact.GetComponent<Renderer>().material = projectileMaterial;

        Destroy(impact, 0.1f);
    }

    private void HandleHit(RaycastHit hit)
    {
        // Apply force
        if (hit.rigidbody != null)
            hit.rigidbody.AddForce(hit.normal * beamForce, ForceMode.Impulse);

        // Destructible objects
        if (hit.collider.CompareTag("Destructible") || hit.collider.CompareTag("Pillar"))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb == null) rb = hit.collider.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            Destroy(hit.collider.gameObject, hit.collider.CompareTag("Destructible") ? 0.5f : 1f);
        }

        // Damage enemies (instead of stun)
        Enemy enemyHealth = hit.collider.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(1); // Adjust damage value if needed
            return;
        }

        // Damage Sentry AI
        SentryAI sentry = hit.collider.GetComponent<SentryAI>();
        if (sentry != null)
        {
            
            if (sentry.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddForce(-hit.normal * 5f, ForceMode.Impulse); // small pushback
            }
            return;
        }

        // Damage Wandering AI
        WanderingAI wander = hit.collider.GetComponent<WanderingAI>();
        if (wander != null)
        {
            // Create a TakeDamage function in WanderingAI first
            wander.TakeDamage(1, -hit.normal * 5f);
            return;
        }




        // Break supports
        BreakableSupport support = hit.collider.GetComponent<BreakableSupport>();
        if (support != null) support.Break();
    }
}

