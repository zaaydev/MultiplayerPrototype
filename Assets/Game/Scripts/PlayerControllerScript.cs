using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float movementForce = 10f;
    [SerializeField] private float maxSpeed = 8f;
    
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float jumpForce = 7f;

    private Vector2 movementInput;

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

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;

            playerRigidbody.linearVelocity = new Vector3(
                horizontalVelocity.x,
                playerRigidbody.linearVelocity.y,
                horizontalVelocity.z
            );
        }
    }


}
