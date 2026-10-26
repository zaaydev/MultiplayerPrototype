using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Camera Position")]
    [SerializeField] private float distance = 6f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float lookHeight = 1f;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float touchSensitivity = 0.15f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;

    private float yaw;
    private float pitch = 15f;

    private void Start()
    {
        // Keep the cursor available while testing in the Editor.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector2 lookDelta = Vector2.zero;
        bool touchIsActive = false;

        // MOBILE: Read finger swipes.
        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (!touch.press.isPressed)
                    continue;

                touchIsActive = true;

                // Where did this finger initially touch the screen?
                Vector2 startPosition =
                    touch.startPosition.ReadValue();

                // Reserve the lower screen for movement,
                // jump, and dash buttons.
                bool isCameraTouch =
                    startPosition.x > Screen.width * 0.45f &&
                    startPosition.y > Screen.height * 0.30f;

                if (isCameraTouch)
                {
                    lookDelta += touch.delta.ReadValue();
                }
            }
        }

        if (touchIsActive)
        {
            // Apply touch sensitivity.
            lookDelta *= touchSensitivity;
        }
        else if (Mouse.current != null)
        {
            // PC: Read mouse movement.
            lookDelta = Mouse.current.delta.ReadValue()
                        * mouseSensitivity;
        }

        // Update camera rotation.
        yaw += lookDelta.x;
        pitch -= lookDelta.y;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation =
            Quaternion.Euler(pitch, yaw, 0f);

        // Follow the player.
        Vector3 focusPoint =
            target.position + Vector3.up * lookHeight;

        Vector3 offset =
            rotation * new Vector3(0f, height, -distance);

        transform.position = focusPoint + offset;

        transform.LookAt(focusPoint);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}