using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Animation Settings")]
    [SerializeField] private float movementThreshold = 0.1f; // Adjust sensitivity for running detection

    void Update()
    {
        HandleAnimations();
    }

    void HandleAnimations()
    {
        // Get movement speed
        float speed = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z).magnitude;
        bool isFalling = !playerMovement.IsGrounded();

        // Set animation states
        animator.SetBool("isRunning", speed > movementThreshold && !isFalling);
        animator.SetBool("isFalling", isFalling);
    }
}
