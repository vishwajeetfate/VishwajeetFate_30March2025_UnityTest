using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Gravity Settings")]
    private Rigidbody rb;
    private GravityManager gravityManager;
    private Vector3 gravityDirection = Vector3.down; // Default gravity
    private Quaternion targetRotation; // Target rotation for smooth transition

    private bool isGrounded; // Ground check

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable default Unity gravity

        gravityManager = FindObjectOfType<GravityManager>();
        if (gravityManager != null)
        {
            gravityDirection = gravityManager.GetGravityDirection();
        }

        AlignPlayerWithGravity();
    }

    void Update()
    {
        HandleGravityChange();
        MovePlayer();
        Jump();
    }

    void FixedUpdate()
    {
        ApplyCustomGravity();
    }

    void HandleGravityChange()
    {
        if (gravityManager != null)
        {
            Vector3 newGravityDirection = gravityManager.GetGravityDirection();
            if (newGravityDirection != gravityDirection)
            {
                gravityDirection = newGravityDirection;
                AlignPlayerWithGravity();
            }
        }

        // Smooth rotation towards the new gravity direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D (Left/Right)
        float vertical = Input.GetAxis("Vertical"); // W/S (Up/Down)

        // Get the current "up" direction from gravity
        Vector3 playerUp = -gravityDirection;

        // Determine "right" relative to the player's current facing direction
        Vector3 moveRight = Vector3.Cross(playerUp, transform.forward).normalized;

        // If moveRight is too small (meaning forward is nearly parallel to gravity), adjust
        if (moveRight.magnitude < 0.1f)
        {
            moveRight = Vector3.Cross(playerUp, transform.right).normalized;
        }

        // "Forward" should always be perpendicular to gravity and right
        Vector3 moveForward = Vector3.Cross(moveRight, playerUp).normalized;

        // Movement based on input
        Vector3 moveDirection = (moveRight * horizontal + moveForward * vertical).normalized;

        // Apply movement while preserving gravity effect
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = moveVelocity + Vector3.Project(rb.velocity, gravityDirection);
    }


    void ApplyCustomGravity()
    {
        rb.AddForce(gravityDirection * Physics.gravity.magnitude, ForceMode.Acceleration);
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity += -gravityDirection * jumpForce;
        }
    }

    public void AlignPlayerWithGravity()
    {
        // Correctly set new "up" direction
        Vector3 newUp = -gravityDirection;

        // Choose a stable "forward" that is not parallel to gravity
        Vector3 referenceForward = transform.forward;
        if (Vector3.Dot(referenceForward, newUp) > 0.9f)
        {
            referenceForward = transform.right; // If forward is too close to up, use right instead
        }

        // Compute the new forward direction based on gravity
        Vector3 newForward = Vector3.Cross(newUp, referenceForward).normalized;

        // Compute the final rotation
        targetRotation = Quaternion.LookRotation(newForward, newUp);

        // Apply the new rotation immediately
        transform.rotation = targetRotation;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = CheckGrounded(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    bool CheckGrounded(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, -gravityDirection) > 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
