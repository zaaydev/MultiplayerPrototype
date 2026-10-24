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

        Vector2 WASD_input = playerActions.PlayerMap.MoveAction.ReadValue<Vector2>();
        controllerScript.ProcessMove(WASD_input);
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
