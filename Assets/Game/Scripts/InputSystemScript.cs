using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemScript : MonoBehaviour
{
    private PlayerInputActions playerActions;
    [SerializeField] private PlayerControllerScript controllerScript;

    private void OnEnable()
    {
        playerActions = new PlayerInputActions();   
        playerActions.PlayerMap.Enable();     
        playerActions.PlayerMap.Jump.performed += TriggerProcessJump;     
    }

    private void Update()
    {
        Vector2 WASD_input = playerActions.PlayerMap.MoveAction.ReadValue<Vector2>();        
        controllerScript.ProcessMove(WASD_input);
    }

    private void TriggerProcessJump(InputAction.CallbackContext ctx) {
        controllerScript.ProcessJump();
    }

    private void OnDisable()
    {
        playerActions.PlayerMap.Jump.performed -= TriggerProcessJump;
        playerActions.PlayerMap.Disable();
    }
}
