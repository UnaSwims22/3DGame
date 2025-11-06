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
    public ChargeUpController chargeController;
    public MuzzleFlash muzzleFlash;


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
            chargeController.BeginCharge();


        if (Input.GetMouseButtonDown(0))
        {
            bool fullPower = chargeController.ReleaseCharge();
            FireRay(fullPower);
        }
            
    }

    void FireRay(bool fullPower)
    {
        muzzleFlash?.PlayFlash();

        if (fullPower)
        {
            beamWidth *= 1.8f;
            beamForce *= 1.6f;
        }
        
        Vector3 screenCenter = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray cameraRay = _camera.ScreenPointToRay(screenCenter);

        Vector3 hitPoint;

        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, beamRange))
        {
            hitPoint = hitInfo.point;
            StartCoroutine(FireBeam3D(hitPoint));
            HandleHit(hitInfo);
        }
        else
        {
            hitPoint = cameraRay.GetPoint(beamRange);
            StartCoroutine(FireBeam3D(hitPoint));
        }

       
        if (fireballPrefab != null)
        {
            GameObject proj = Instantiate(
                fireballPrefab,
                firePoint.position,
                Quaternion.LookRotation(cameraRay.direction)
            );

            if (projectileMaterial != null)
                proj.GetComponent<Renderer>().material = projectileMaterial;

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = cameraRay.direction * projectileSpeed;
            }

            Destroy(proj, 3f);
        }
    }


    private IEnumerator FireBeam3D(Vector3 targetPos)
    {
        GameObject beamObj = new GameObject("RayBeamMesh");
        MeshFilter meshFilter = beamObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = beamObj.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Unlit/Color"));
        meshRenderer.material.color = beamColor;

        Mesh beamMesh = new Mesh();
        meshFilter.mesh = beamMesh;

        Vector3 startPos = firePoint.position;
        Vector3 dir = (targetPos - startPos).normalized;
        float dist = Vector3.Distance(startPos, targetPos);

        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / travelTime);

            Vector3 currentEnd = Vector3.Lerp(startPos, targetPos, t);
            UpdateBeamMesh(beamMesh, startPos, currentEnd);

            yield return null;
        }

        UpdateBeamMesh(beamMesh, startPos, targetPos);

        yield return new WaitForSeconds(beamHoldTime);
        Destroy(beamObj);
    }

   
    void UpdateBeamMesh(Mesh mesh, Vector3 start, Vector3 end)
    {
        Vector3 dir = (end - start).normalized;
        Vector3 side = Vector3.Cross(dir, Vector3.up).normalized * beamWidth;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = start + side;
        vertices[1] = start - side;
        vertices[2] = end + side;
        vertices[3] = end - side;

        int[] triangles = new int[]
        {
            0, 2, 1,
            2, 3, 1
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
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

        // Damage enemies
        Enemy enemyHealth = hit.collider.GetComponentInParent<Enemy>();
        if (enemyHealth != null)
        {
            Vector3 push = -hit.normal * 6f;
            enemyHealth.TakeDamage(1, push);
            return;
        }

        // Damage Sentry AI
        SentryAI sentry = hit.collider.GetComponent<SentryAI>();
        if (sentry != null)
        {
            if (sentry.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.AddForce(-hit.normal * 5f, ForceMode.Impulse);
            return;
        }

        // Damage Wandering AI
        WanderingAI wander = hit.collider.GetComponent<WanderingAI>();
        if (wander != null)
        {
            wander.TakeDamage(1, -hit.normal * 5f);
            return;
        }

        BreakableSupport support = hit.collider.GetComponent<BreakableSupport>();
        if (support != null) support.Break();
    }
}
