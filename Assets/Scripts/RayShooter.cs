using Unity.Mathematics;
using UnityEngine;
using System.Collections;

//Title: Creating a rayGun
//Author:Hocking, J.
//Date: 2015
//Code Version:
//Availability: Unity in Action (Textbook)
public class RayShooter : MonoBehaviour
{
    [Header("RayGun References")]
    public Camera _camera;
    public Transform gunTransform;
    public Transform firePoint;      // Empty GameObject at muzzle

    [Header("Laser Settings")]
    public float beamWidth = 0.05f;
    public Color beamColor = Color.red;
    public float travelTime = 0.2f;  // Time for beam to reach target
    public float beamHoldTime = 0.05f; // Time beam stays fully visible

    void Start()
    {
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireRay();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            FireRay();
        }
    }

    private void FireRay()
    {
        Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        Ray ray = _camera.ScreenPointToRay(point);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Rotate gun to face hit point
            if (gunTransform != null)
            {
                Vector3 lookDirection = hit.point - gunTransform.position;
                gunTransform.rotation = Quaternion.LookRotation(lookDirection);
            }

            // Start visual effects
            StartCoroutine(ShootLaser(hit.point));

            // Target reaction (optional gameplay logic)
            GameObject hitObject = hit.transform.gameObject;
            ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
            if (target != null)
            {
                target.ReactToHit();
            }
        }
    }
 

    private IEnumerator ShootLaser(Vector3 hitPos)
    {
        // Decide start position (firePoint or fallback to camera)
        Vector3 startPos = firePoint != null ? firePoint.position : _camera.transform.position;

        // Create LineRenderer dynamically
        GameObject lineObj = new GameObject("LaserBeam");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // Configure LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = beamColor;

        // Animate beam travel
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

        // Hold beam briefly at full length
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, hitPos);
        yield return new WaitForSeconds(beamHoldTime);

        // Destroy beam
        Destroy(lineObj);

        // Show sphere impact AFTER beam arrives
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = hitPos;
        yield return new WaitForSeconds(1f);
        Destroy(sphere);
    }


    private IEnumerator SphereIndicator(Vector3 pos)
    {

        // Creating the sphere indicator
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;


        // Create a GameObject to hold the LineRenderer
        GameObject lineObj = new GameObject("LaserBeam");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // Configure LineRenderer
        lineRenderer.positionCount = 2; // start + end
        lineRenderer.SetPosition(0, _camera.transform.position); // start from camera (or gun)
        lineRenderer.SetPosition(1, pos); // end at hit position

        // Styling the line
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Use a simple material (optional: assign a laser material in Inspector)
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.red;

        // Keep beam for 0.1 seconds to simulate a "projectile laser"
        yield return new WaitForSeconds(0.1f);
        Destroy(lineObj);

        // Keep sphere for 1 second as before
        yield return new WaitForSeconds(0.9f);
        Destroy(sphere);
    }
 

    void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");


    }
}
