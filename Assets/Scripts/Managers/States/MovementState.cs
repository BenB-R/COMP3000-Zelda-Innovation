using UnityEngine;

public class MovementState : IState
{
    private PlayerController playerController;
    private PlayerInputs inputActions;
    private CameraController cameraController;

    public MovementState(PlayerController controller, PlayerInputs inputActions, CameraController cameraController)
    {
        playerController = controller;
        this.inputActions = inputActions;
        this.cameraController = cameraController;
    }

    public void Enter()
    {
        Debug.Log("Entered Movement State");
        // Hide the cursor and lock it to the center of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Execute()
    {
        Vector2 movementInput = inputActions.BasicMovement.Move.ReadValue<Vector2>();
        Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y);

        // Read the mouse movement components separately
        float mouseX = inputActions.BasicMovement.CameraX.ReadValue<float>();
        float mouseY = inputActions.BasicMovement.CameraY.ReadValue<float>();

        Vector2 mouseMovement = new Vector2(mouseX, mouseY);

        playerController.MovePlayer(movementDirection);
        cameraController.HandleMouseInput(mouseMovement);

        if (movementDirection != Vector3.zero)
        {
            Debug.Log($"Executing movement with direction: {movementDirection}");
        }

        if (inputActions.BasicMovement.Jump.WasPressedThisFrame())
        {
            playerController.Jump();
            Debug.Log("Jump executed");
        }

        if (inputActions.BasicMovement.Interact.WasPressedThisFrame())
        {
            playerController.Interact();
            Debug.Log("Interact executed");
        }
    }

    public void Exit()
    {
        Debug.Log("Exited Movement State");
        // Show the cursor and unlock it when exiting the movement state
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}