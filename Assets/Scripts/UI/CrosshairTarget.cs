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

    }

    void UpdateCrosshairPosition()
    {

    }
}
    
