using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float movementForce = 10f;
    [SerializeField] private float maxSpeed = 8f;
    private Vector2 movementInput;
    
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float jumpForce = 7f;

    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float maxDashingForce = 20f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float nextDashTime;
    private float dashingDuration = 0.3f;


    public void ProcessMove(Vector2 WASD_input) {
        movementInput = WASD_input;
    }

    public void ProcessJump()
    {
        if (!isGrounded) return;

        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void ProcessDash() {
        if (Time.time < nextDashTime) return;

        // find dashing direction
        Vector3 dashDirection = new Vector3(movementInput.x, 0, movementInput.y);

        if (dashDirection == Vector3.zero) return;
        playerRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        isDashing = true;
        Invoke(nameof(StopDashingDuration), dashingDuration);

        nextDashTime = Time.time + dashCooldown;        
    }
    private void StopDashingDuration()
    {
        isDashing = false;
    }


    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(
            movementInput.x, 0f, movementInput.y
        ).normalized;

        playerRigidbody.AddForce(moveDirection * movementForce);

        Vector3 horizontalVelocity = new Vector3(
            playerRigidbody.linearVelocity.x,
            0f,
            playerRigidbody.linearVelocity.z
        );

        float allowedMaxSpeed = isDashing ? maxDashingForce : maxSpeed;  
        if (horizontalVelocity.magnitude > allowedMaxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * allowedMaxSpeed;

            playerRigidbody.linearVelocity = new Vector3(
                horizontalVelocity.x,
                playerRigidbody.linearVelocity.y,
                horizontalVelocity.z
            );
        }
    }


}
