using UnityEngine;
using UnityEngine.UI;

public class CrosshairTarget : MonoBehaviour
{
    public Camera _camera;
    public Image crosshair;
    public float maxDistance = 100f;
    public LayerMask aimLayerMask = Physics.DefaultRaycastLayers;

    [HideInInspector]
    public Vector3 aimPoint;

    void Update()
    {
        UpdateAimPoint();
        UpdateCrosshairPosition();
    }

    void UpdateAimPoint()
    {
        // Cast ray from center of screen
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = _camera.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, aimLayerMask))
        {
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = ray.GetPoint(maxDistance);
        }
    }

    void UpdateCrosshairPosition()
    {

    }
}
    
