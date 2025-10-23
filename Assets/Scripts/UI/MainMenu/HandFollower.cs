using UnityEngine;
using UnityEngine.UI;

public class HandFollower : MonoBehaviour
{
    public MenuButtonController menuButtonController;
    public RectTransform[] buttonPositions; //Assign each buttons RectTransform
    public float moveSpeed = 5f;
    public Vector3 offset = new Vector3(-150f, 0, 0);

    private Vector3 startPosition;
    private bool hasStartedFollowing = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (buttonPositions.Length == 0  ||  menuButtonController == null) return;

        int targetIndex = menuButtonController.index;

        Vector3 targetPos = buttonPositions[targetIndex].position + offset;

        if (!hasStartedFollowing)
        {
            hasStartedFollowing = true;
            transform.position = startPosition;
        }
        
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }
}
