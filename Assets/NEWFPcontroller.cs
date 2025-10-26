using UnityEngine;
using UnityEngine.InputSystem;

public class NEWFPcontroller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2f;
    private float originalMoveSpeed;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    public float throwForce = 10f;
    public float throwUpwardBoost = 1f;
    private PickUpObject heldObject;

    private CharacterController controller;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private bool isGrounded;
    private bool isCrouching;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();

        if (heldObject != null)
            heldObject.MoveToHoldPoint(holdPoint.position);

        // Animator sync
        animator.SetBool("isRunning", moveInput.y != 0 || moveInput.x != 0);
        animator.SetBool("isJumping", !controller.isGrounded);
        animator.SetBool("isCrouching", isCrouching);
    }

    // ===== Input Callbacks (New Input System) =====
    public void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();
    public void OnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed;
            isCrouching = true;
        }
        else if (context.canceled)
        {
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
            isCrouching = false;
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

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed || heldObject == null) return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        heldObject.Drop();
        rb.AddForce(cameraTransform.forward * throwForce + Vector3.up * throwUpwardBoost, ForceMode.Impulse);
        heldObject = null;
    }

    // ===== Core Mechanics =====
    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

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
}

