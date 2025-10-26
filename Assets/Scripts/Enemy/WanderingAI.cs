using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

//Title: Creating wandering AI
//Author:Hocking, J.
//Date: 2015
//Code Version:
//Availability: Unity in Action (Textbook)

public class WanderingAI : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private GameObject fireballPrefab;
    private GameObject _fireball;

    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    public float attackCooldown = 2.0f;
    public float detectionRange = 15f;

    private float lastAttackTime;
    private bool _alive = true;

    [Header("Player Reference")]
    private Transform player;

    void Start()
    {
        _alive = true;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!_alive) return;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Follow the player
            if (distanceToPlayer < detectionRange)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                dir.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

                // Attack if cooldown expired
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    ShootAtPlayer();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                // Wander randomly when player not nearby
                transform.Translate(0, 0, speed * Time.deltaTime);

                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.SphereCast(ray, 0.75f, out RaycastHit hit))
                {
                    if (hit.distance < obstacleRange)
                    {
                        float angle = Random.Range(-110, 110);
                        transform.Rotate(0, angle, 0);
                    }
                }
            }
        }
    }

    private void ShootAtPlayer()
    {
        if (fireballPrefab != null)
        {
            _fireball = Instantiate(fireballPrefab, transform.TransformPoint(Vector3.forward * 1.5f), transform.rotation);
            Rigidbody rb = _fireball.GetComponent<Rigidbody>();
            Collider fireballCollider = _fireball.GetComponent<Collider>();
            Collider aiCollider = GetComponent<Collider>();


            if (fireballCollider != null && aiCollider != null)
            {
                // Prevent fireball from colliding with the AI who fired it
                Physics.IgnoreCollision(fireballCollider, aiCollider);
            }


            if (rb != null && player != null)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                rb.linearVelocity = dir * 25f;
            }

            Destroy(_fireball, 5f);
        }

        ProjectileDamage projectileDamage = _fireball.GetComponent<ProjectileDamage>();
        if (projectileDamage != null)
        {
            projectileDamage.damage = 10f;       // or whatever feels balanced
            projectileDamage.targetTag = "Player";
        }

    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }
}
