using System.Runtime.InteropServices;
using UnityEngine;

using UnityEngine.InputSystem;

//Title: Creating FPControls
//Author: Hayes, A.
//Code Version: 2.5
//Availability: Ulwazi
public class FPController : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    // Jumping
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Camera _camera;
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    [SerializeField] private Animator animator;
    private CharacterController CharacterController => controller;

    // Crouching
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2f;
    private float originalMoveSpeed;
    private bool isCrouching = false;

    [Header("PickUp Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 1f;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();

        // Keep held item following the hold point
        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }

        UpdateAnimationStates();
    }

    // -------------------- INPUT METHODS --------------------
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = true;
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed;
        }
        else if (context.canceled)
        {
            isCrouching = false;
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject == null)
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();
                if (pickUp != null)
                {
                    pickUp.PickUp(holdPoint);
                    heldObject = pickUp;
                }
                else
                {
                    Debug.Log("Object hit has no PickUpObject component: " + hit.collider.name);
                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }
    }

    // -------------------- CORE MOVEMENT --------------------
    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false); // reset jump when grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // -------------------- ANIMATION LOGIC --------------------
    private void UpdateAnimationStates()
    {
        // Walking: check if the player is moving on the ground
        bool isWalking = controller.isGrounded && moveInput.magnitude > 0.1f && !isCrouching;
        animator.SetBool("isWalking", isWalking);

        // Crouching: use our state variable
        animator.SetBool("isCrouching", isCrouching);
    }
}
