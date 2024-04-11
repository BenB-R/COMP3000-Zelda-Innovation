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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Execute()
    {
        Vector2 movementInput = inputActions.BasicMovement.Move.ReadValue<Vector2>();
        Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y);
        float mouseX = inputActions.BasicMovement.CameraX.ReadValue<float>();
        float mouseY = inputActions.BasicMovement.CameraY.ReadValue<float>();
        Vector2 mouseMovement = new Vector2(mouseX, mouseY);

        playerController.MovePlayer(movementDirection);
        cameraController.HandleMouseInput(mouseMovement);

        if (inputActions.BasicMovement.Jump.WasPressedThisFrame()) playerController.Jump();
        else if (inputActions.BasicMovement.Jump.WasReleasedThisFrame()) playerController.OnJumpButtonReleased();
        if (inputActions.BasicMovement.Interact.WasPressedThisFrame()) playerController.Interact();

        bool isSprinting = inputActions.BasicMovement.Sprint.ReadValue<float>() > 0;
        if (isSprinting) playerController.Sprint();
        else playerController.StopSprint();
    }

    public void Exit()
    {
        Debug.Log("Exited Movement State");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
