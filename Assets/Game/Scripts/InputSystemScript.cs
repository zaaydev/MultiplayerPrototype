using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

public class InputSystemScript : NetworkBehaviour
{
    private PlayerInputActions playerActions;
    private PlayerControllerScript controllerScript;

    public override void Spawned()
    {
        if (!Object.HasStateAuthority)
        {
            enabled = false;
            return;
        }

        controllerScript = GetComponent<PlayerControllerScript>();

        playerActions = new PlayerInputActions();
        playerActions.PlayerMap.Enable();
        playerActions.PlayerMap.Jump.performed += TriggerProcessJump;
        playerActions.PlayerMap.Dash.performed += TriggerProcessDash;
    }

    private void Update()
    {
        if (playerActions == null || controllerScript == null) return;

        Vector2 input = playerActions.PlayerMap.MoveAction.ReadValue<Vector2>();

        Transform cam = Camera.main.transform;

        Vector3 cameraForward = cam.forward;
        Vector3 cameraRight = cam.right;

        // Keep movement on the ground plane.
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection =
            cameraForward * input.y +
            cameraRight * input.x;

        controllerScript.ProcessMove(
            new Vector2(moveDirection.x, moveDirection.z)
        );
    }

    private void TriggerProcessJump(InputAction.CallbackContext ctx)
    {
        controllerScript.ProcessJump();
    }
    private void TriggerProcessDash(InputAction.CallbackContext ctx)
    {
        controllerScript.ProcessDash();
    }

    private void OnDisable()
    {
        if (playerActions == null) return;

        playerActions.PlayerMap.Jump.performed -= TriggerProcessJump;
        playerActions.PlayerMap.Dash.performed -= TriggerProcessDash;
        playerActions.PlayerMap.Disable();
        playerActions.Dispose();
        playerActions = null;
    }
}
