using UnityEngine;

public class InputSystemScript : MonoBehaviour
{
    private PlayerInputActions playerActions;
    [SerializeField] private PlayerControllerScript controllerScript;

    private void OnEnable()
    {
        playerActions = new PlayerInputActions();   
        playerActions.PlayerMap.Enable();     
    }

    private void Update()
    {
        Vector2 WASD_input = playerActions.PlayerMap.MoveAction.ReadValue<Vector2>();        
        controllerScript.ProcessMove(WASD_input);
    }

    private void OnDisable()
    {
        playerActions.PlayerMap.Disable();
    }
}
