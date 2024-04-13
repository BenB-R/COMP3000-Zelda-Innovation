using UnityEngine;
using UnityEngine.InputSystem;

public class CombatState : IState
{
    private PlayerController playerController;
    private PlayerInputs inputActions;
    private CameraController cameraController;

    public CombatState(PlayerController controller, PlayerInputs inputActions, CameraController cameraController)
    {
        playerController = controller;
        this.inputActions = inputActions;
        this.cameraController = cameraController;
    }

    public void Enter()
    {
        Debug.Log("Entered Combat State");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inputActions.Combat.Enable();  // Ensure combat inputs are enabled
    }

    public void Execute()
    {
        // Handle movement even during combat
        Vector2 movementInput = inputActions.BasicMovement.Move.ReadValue<Vector2>();
        Vector3 movementVector = new Vector3(movementInput.x, 0, movementInput.y);
        playerController.CombatMovement(movementVector);

        // Directly handle combat actions
        if (inputActions.Combat.SmallAttack.WasPressedThisFrame())
        {
            playerController.PerformSmallAttack();
        }
        if (inputActions.Combat.BigAttack.WasPressedThisFrame())
        {
            playerController.PerformBigAttack();
        }

        // Example of handling target switching
        if (inputActions.Combat.SwitchLeft.WasPressedThisFrame())
        {
            playerController.SwitchTarget(true);
        }
        if (inputActions.Combat.SwitchRight.WasPressedThisFrame())
        {
            playerController.SwitchTarget(false);
        }
    }

    public void Exit()
    {
        Debug.Log("Exited Combat State");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        inputActions.Combat.Disable();  // Disable combat inputs on exit
    }
}
