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
    private Camera _camera;
    private Controls controls;


    void Awake()
    {
        _camera = GetComponent<Camera>();
        controls = new Controls();
       
    }


    void Start()
    {
        _camera = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                StartCoroutine(SphereIndicator(hit.point));
            }
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    target.ReactToHit();

                }
                else
                {
                    StartCoroutine(SphereIndicator(hit.point));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                StartCoroutine(SphereIndicator(hit.point));
            }
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    target.ReactToHit();

                }
                else
                {
                    StartCoroutine(SphereIndicator(hit.point));
                }
            }
        }
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

        // Style the line (adjust to taste)
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
