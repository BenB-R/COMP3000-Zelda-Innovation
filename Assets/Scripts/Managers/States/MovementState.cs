using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : IState
{
    private PlayerController playerController;
    private PlayerInputs playerInputs;
    private Vector2 currentMovementInput;
    private bool isJumping;
    private bool isInteracting;

    public MovementState(PlayerController controller)
    {
        playerController = controller;
        playerInputs = new PlayerInputs();

        // Movement
        playerInputs.BasicMovement.Move.performed += ctx =>
        {
            currentMovementInput = ctx.ReadValue<Vector2>();
            Debug.Log($"Move performed with input: {currentMovementInput}");
        };
        playerInputs.BasicMovement.Move.canceled += ctx =>
        {
            currentMovementInput = Vector2.zero;
            Debug.Log("Move canceled");
        };

        // Interact
        playerInputs.BasicMovement.Interact.performed += ctx =>
        {
            // This will trigger when the 'E' key is initially pressed down
            isInteracting = false;
            Debug.Log("Interact button pressed");
        };
        playerInputs.BasicMovement.Interact.canceled += ctx =>
        {
            // This will trigger when the 'E' key is released
            isInteracting = true;
            Debug.Log("Interacted on key release");
        };

        // Jump
        playerInputs.BasicMovement.Jump.performed += ctx =>
        {
            isJumping = ctx.ReadValueAsButton();
            Debug.Log("Jump performed");
        };
    }

    public void Enter()
    {
        playerInputs.BasicMovement.Enable();
        playerInputs.Gliding.Enable();
        Debug.Log("Entered Movement State");
        // Hide the cursor and lock it to the center of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Execute()
    {
        Vector3 movementDirection = new Vector3(currentMovementInput.x, 0, currentMovementInput.y);
        playerController.Move(movementDirection);

        if (movementDirection != Vector3.zero)
        {
            Debug.Log($"Executing movement with direction: {movementDirection}");
        }

        if (isJumping)
        {
            playerController.Jump();
            isJumping = false;
            Debug.Log("Jump executed");
        }

        if (isInteracting)
        {
            playerController.Interact();
            Debug.Log("Interact executed");
            isInteracting = false;
        }
    }

    public void Exit()
    {
        playerInputs.BasicMovement.Disable();
        playerInputs.Gliding.Disable();
        Debug.Log("Exited Movement State");
        // Show the cursor and unlock it when exiting the movement state
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
