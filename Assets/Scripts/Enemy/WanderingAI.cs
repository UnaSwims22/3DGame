using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    public float speed = 3.0f;  // movement speed of AI
    public float obstacleRange = 5.0f; //reaction range

    private bool _alive;
    void Start()
    {
        _alive = true;
    }

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.75f, out hit))
        {
            if (hit.distance < obstacleRange)
            {
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }
        if (_alive)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
    public void SetAlive(bool alive)
    {
        _alive = alive;
    }
}
