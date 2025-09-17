using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using UnityEditor.PackageManager;

//Title: Creating a rayGun (Optimized)
//Author: Based on Hocking, J.
//Date: 2015
//Code Version:
//Availability: Unity in Action (Textbook)


public class RayShooter : MonoBehaviour
{
    [Header("RayGun References")]
    public Camera _camera;
    public CrosshairTarget crosshairTarget;
    private Controls controls;
    public Transform gunTransform;
    public Transform firePoint;
    public GameObject fireballPrefab;

    [Header("Projectile & Beam Settings")]
    public float projectileSpeed = 30f;
    public float beamWidth = 0.05f;
    public Color beamColor = Color.red;
    public float travelTime = 0.2f;  // Time for beam to reach target
    public float beamHoldTime = 0.05f; // Time beam stays visible
    public float beamRange = 100f;
    public float beamForce = 300f;


    void Start()
    {
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
            controls = new Controls();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        AimGunAtCrosshair();

        if (Input.GetMouseButtonDown(0))
        {
            FireRay();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            FireRay();
        }
    }

    private void AimGunAtCrosshair()
    {
        // Rotate gun to face hit point
        if (gunTransform != null)
        {
            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray cameraRay = _camera.ScreenPointToRay(point);

            Vector3 aimPoint = cameraRay.GetPoint(beamRange);

            Vector3 lookDirection = (aimPoint - gunTransform.position).normalized;
            gunTransform.rotation = Quaternion.LookRotation(lookDirection);
        }

    }

    private void FireRay()
    {
        Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray cameraRay = _camera.ScreenPointToRay(point);

        RaycastHit hitInfo;
        Vector3 targetPoint;

        if (Physics.Raycast(cameraRay, out hitInfo, 100f))
        {
            targetPoint = hitInfo.point;
        }
        else
        {

            targetPoint = cameraRay.GetPoint(100f);
        }

        //  direction from gun muzzle to target point
        Vector3 beamDirection = (targetPoint - firePoint.position).normalized;


        {
            StartCoroutine(SphereIndicator(targetPoint));

            if (fireballPrefab != null && firePoint != null)
            {
                Vector3 direction = (targetPoint - firePoint.position).normalized;
                GameObject projectile = Instantiate(fireballPrefab, firePoint.position, Quaternion.LookRotation(direction));
                Rigidbody br = projectile.GetComponent<Rigidbody>();
                if (br != null)
                {
                    br.linearVelocity = direction * projectileSpeed;
                }
            }

            StartCoroutine(ShootLaser(targetPoint));

            if (hitInfo.collider != null)
            {
                Rigidbody rb = hitInfo.collider.attachedRigidbody;
                if (rb != null)
                {
                    rb.AddForce(cameraRay.direction * beamForce, ForceMode.Impulse);
                }

                BreakableSupport support = hitInfo.collider.GetComponent<BreakableSupport>();
                if (support != null)
                {
                    support.Break();
                }

                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Stun();
                }


                HandleHit(hitInfo);

                Shootable shootable = hitInfo.transform.GetComponent<Shootable>();
                if (shootable != null)
                {
                    shootable.ReactToHit(hitInfo.point);
                }

                // Target reaction 
                GameObject hitObject = hitInfo.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    target.ReactToHit();
                }

            }
        }
    }

    private void HandleHit(RaycastHit hitInfo)
    {
        GameObject hitObject = hitInfo.collider.gameObject;


        if (hitObject.CompareTag("Destructible"))
        {
            // Trigger physics so object falls
            Rigidbody rb = hitObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = hitObject.AddComponent<Rigidbody>();
            }
            rb.useGravity = true;
            rb.isKinematic = false;


            Destroy(hitObject, .5f);
        }

        // pillar, make it collapse 
        if (hitObject.CompareTag("Pillar"))
        {
            Rigidbody rb = hitObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = hitObject.AddComponent<Rigidbody>();
            }
            rb.useGravity = true;
            rb.isKinematic = false;
            Destroy(hitObject, 1.6f);
        }

    }

    private IEnumerator ShootLaser(Vector3 hitPos)
    {

        Vector3 startPos = firePoint.position;


        GameObject lineObj = new GameObject("LaserBeam");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // LineRenderer configuration
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = beamColor;

        // Animating beam travel
        float elapsed = 0f;
        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / travelTime);
            Vector3 currentPos = Vector3.Lerp(startPos, hitPos, t);

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, currentPos);

            yield return null;
        }

        // Hold beam for a moment
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, hitPos);
        yield return new WaitForSeconds(beamHoldTime);


        Destroy(lineObj);

        // Show sphere impact AFTER beam arrives
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = hitPos;
        yield return new WaitForSeconds(0.2f);
        Destroy(sphere);
    }


    private IEnumerator SphereIndicator(Vector3 pos)
    {

        // sphere indicator
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;



        GameObject lineObj = new GameObject("LaserBeam");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // Configure LineRenderer
        lineRenderer.positionCount = 2; // start + end
        lineRenderer.SetPosition(0, firePoint.position); // start from camera (or gun)
        lineRenderer.SetPosition(1, pos); // end at hit position

        // Styling the line
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;


        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.red;


        yield return new WaitForSeconds(0.1f);
        Destroy(lineObj);


        yield return new WaitForSeconds(0.01f);
        Destroy(sphere);
    }


    
}



