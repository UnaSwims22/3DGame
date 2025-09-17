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


    void Update()
    {
        if (crosshairTarget == null || _camera == null) return; 

        if (gunTransform != null)
        {
            Vector3 aimDirection = (crosshairTarget.aimPoint - gunTransform.position).normalized;
            gunTransform.rotation = Quaternion.LookRotation(aimDirection);

            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray cameraRay = _camera.ScreenPointToRay(point);

            Vector3 aimPoint = cameraRay.GetPoint(beamRange);
        }
        

        //Fire input
        if (Input.GetMouseButtonDown(0)  || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            FireAtAimPoint(crosshairTarget.aimPoint);
        }

    }



    void FireAtAimPoint(Vector3 aimPoint)
    {
        Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray cameraRay = _camera.ScreenPointToRay(point);


        if (firePoint == null) return;
        Vector3 direction = (aimPoint - firePoint.position).normalized;

        {
            GameObject projectile = Instantiate(fireballPrefab, firePoint.position, Quaternion.LookRotation(direction));
            Rigidbody br = projectile.GetComponent<Rigidbody>();
            if (br != null)
            {
                br.linearVelocity = direction * projectileSpeed;
            }
        }

        RaycastHit hitInfo;
        Vector3 targetPoint;

        if (Physics.Raycast(_camera.transform.position, direction, out hitInfo, 100f))
        {
            HandleHit(hitInfo);
            StartCoroutine(ShootLaser(hitInfo.point)); targetPoint = hitInfo.point;
        }
        else
        {

            StartCoroutine(ShootLaser(firePoint.position + direction * 100f));
        }


        //  direction from gun muzzle to target point
        


        {
            StartCoroutine(SphereIndicator(aimPoint));

            if (fireballPrefab != null && firePoint != null)
            {
                Vector3 beamDirection = (aimPoint - firePoint.position).normalized;
                GameObject projectile = Instantiate(fireballPrefab, firePoint.position, Quaternion.LookRotation(direction));
                Rigidbody br = projectile.GetComponent<Rigidbody>();
                if (br != null)
                {
                    br.linearVelocity = direction * projectileSpeed;
                }
            }

            StartCoroutine(ShootLaser(aimPoint));

            if (hitInfo.collider != null)
            {
                Rigidbody rb = hitInfo.collider.attachedRigidbody;
                if (rb != null)
                {
                    rb.AddForce(cameraRay.direction * beamForce, ForceMode.Impulse);
                }



                HandleHit(hitInfo);


            }
        }

        void HandleHit(RaycastHit hitInfo)
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            // Apply force
            Rigidbody rb = hitInfo.collider.attachedRigidbody;
            if (rb != null)
            {
                rb.AddForce((hitInfo.point - firePoint.position).normalized);
            }

            // Destructible objects
            if (hitObject.CompareTag("Destructible") || hitObject.CompareTag("Pillar"))
            {
                if (rb == null)
                {
                    rb = hitObject.AddComponent<Rigidbody>();
                }
                rb.useGravity = true;
                rb.isKinematic = false;

                Destroy(hitObject, hitObject.CompareTag("Destructible") ? 2f : 0.6f);


            }

            // Enemy Stun
            Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Stun();
            }

            //Breakable supports
            BreakableSupport support = hitInfo.collider.GetComponent<BreakableSupport>();
            if (support != null)
            {
                support.Break();
            }

            // Shootable reaction
            Shootable shootable = hitInfo.transform.GetComponent<Shootable>();
            if (shootable != null)
            {
                shootable.ReactToHit(hitInfo.point);
            }

            // Reaction  
            ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
            if (target != null)
            {
                target.ReactToHit();
            }
        }

        IEnumerator ShootLaser(Vector3 hitPos)
        {
            if (firePoint == null) yield break;

            Vector3 startPos = firePoint.position;

            GameObject lineObj = new GameObject("LaserBeam");
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();

            // LineRenderer configuration
            lr.positionCount = 2;
            lr.startWidth = beamWidth;
            lr.endWidth = beamWidth;
            lr.material = new Material(Shader.Find("Unlit/Color"));
            lr.material.color = beamColor;

            float elapsed = 0f;
            while (elapsed < travelTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / travelTime);
                Vector3 currentPos = Vector3.Lerp(startPos, hitPos, elapsed / travelTime);
                lr.SetPosition(0, startPos);
                lr.SetPosition(1, currentPos);
                yield return null;
            }

            // Hold beam momentarilly
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, hitPos);
            yield return new WaitForSeconds(beamHoldTime);
            Destroy(lineObj);

            // Sphere impact AFTER beam arrives
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = hitPos;
            yield return new WaitForSeconds(0.2f);
            Destroy(sphere);
        }
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
