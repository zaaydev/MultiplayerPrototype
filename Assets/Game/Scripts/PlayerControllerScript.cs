using UnityEngine;
using System.Collections;
using Fusion;
using UnityEngine.SceneManagement;

public class PlayerControllerScript : NetworkBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float movementForce = 10f;
    [SerializeField] private float maxSpeed = 8f;
    private Vector2 movementInput;

    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashingDuration = 0.3f;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMPro.TMP_Text resultText;
    // afterimage
    [SerializeField] private GameObject afterimagePrefab;
    private Vector3 spawnPosition;
    private Canvas resultCanvas;

    private bool isDashing = false;
    private float nextDashTime;


    public void ProcessMove(Vector2 WASD_input)
    {
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

    public void ProcessDash()
    {
        if (movementInput == Vector2.zero) return;
        if (Time.time < nextDashTime) return;

        StartCoroutine(SpawnAfterimages());

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
    private IEnumerator SpawnAfterimages()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(
                afterimagePrefab,
                transform.position,
                transform.rotation
            );

            yield return new WaitForSeconds(0.06f);
        }
    }

    public override void Spawned()
    {
        spawnPosition = transform.position;
    }

    private void Update()
    {
        if (!Object.HasStateAuthority) return;

        if (transform.position.y < -2f)
        {
            // Stop the ball's movement
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;

            // Respawn at the original position
            playerRigidbody.position = spawnPosition;
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

        float maxDashingSpeed = dashForce;
        float allowedMaxSpeed = isDashing ? maxDashingSpeed : maxSpeed;

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
