using UnityEngine.InputSystem;
using UnityEngine;
using Suntail;
using fyp;

public class StateManager : MonoBehaviour
{
    // Enumeration for different game states
    private MovementState movementState;
    private CombatState combatState;
    [SerializeField] private PlayerController playerController;

    public enum GameState
    {
        Menu,
        NormalMovement,
        Combat
    }

    // The current state of the game
    public GameState currentState;

    // Reference to the input actions (assuming you have this set up)
    private PlayerInputs inputActions;
    private InputAction menuToggleAction; // Reference to the menu toggle action
    private InputAction combatToggleAction;

    void Awake()
    {
        // Initialize the input actions
        inputActions = new PlayerInputs();

        // Get the reference to the menu toggle action (assuming it's in an "UI" action map)
        menuToggleAction = inputActions.General.Menu;

        // Register the menu toggle action
        menuToggleAction.performed += HandleMenuToggle;

        combatToggleAction = inputActions.General.CombatToggle;

        combatToggleAction.performed += HandleCombatToggle;

        // Initialize the game state to NormalMovement
        ChangeState(GameState.NormalMovement);

        var playerController = FindObjectOfType<PlayerController>(); // Assuming there is only one PlayerController in the scene.
        var cameraController = FindObjectOfType<CameraController>();
        if (playerController != null)
        {
            movementState = new MovementState(playerController, inputActions, cameraController);
            combatState = new CombatState(playerController, inputActions, cameraController);

            movementState.Enter(); // Call the Enter method after initializing the MovementState
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }

    // Method to change the current game state
    public void ChangeState(GameState newState)
    {
        Debug.Log($"Changing state from {currentState} to {newState}");
        currentState = newState;
        OnStateChanged(newState);
    }

    // Method called when the game state changes
    private void OnStateChanged(GameState newState)
    {
        DisableAllInputs(); // Disable all inputs/actions

        switch (newState)
        {
            case GameState.Menu:
                Debug.Log("Entering Menu State");
                // Show the cursor and unlock it
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                HandleInputActions(newState); // Enable input actions for the new state
                break;

            case GameState.NormalMovement:
                Debug.Log("Entering Normal Movement State");
                movementState?.Enter();
                HandleInputActions(newState); // Enable input actions for the new state
                                              // Hide the cursor and lock it to the center of the screen
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case GameState.Combat:
                Debug.Log("Entering Combat State");
                combatState?.Enter();
                HandleInputActions(newState);

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    // Method to disable all inputs/actions
    private void DisableAllInputs()
    {
        Debug.Log("Disabling all inputs");
        // disable all input action maps
        inputActions.BasicMovement.Disable();
        inputActions.InMenu.Disable();
        inputActions.Combat.Disable();
    }

    // Method to toggle the menu state
    private void HandleMenuToggle(InputAction.CallbackContext context)
    {
        Debug.Log("Combat Toggle Pressed");
        if (currentState == GameState.NormalMovement)
        {
            ChangeState(GameState.Menu);
        }
        else if (currentState == GameState.Menu)
        {
            ChangeState(GameState.NormalMovement);
        }
    }

    private void HandleCombatToggle(InputAction.CallbackContext context)
    {
        if (currentState == GameState.NormalMovement)
        {
            if (playerController.CanLockOntoTarget())
            {
                Debug.Log("Target found, switching to combat state.");
                ChangeState(GameState.Combat);
                playerController.ToggleLockOn();
            }
        }
        else if (currentState == GameState.Combat)
        {
            ChangeState(GameState.NormalMovement);
            // Optionally, disable lock-on when exiting combat state.
            playerController.ToggleLockOn(); 
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.NormalMovement:
                movementState?.Execute();
                break;
            case GameState.Combat:
                combatState?.Execute();
                break;
            case GameState.Menu:
                // Menu state logic
                break;
        }
    }

    private void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.Enable(); // Enable the input actions
        }
    }

    private void OnDisable()
    {
        inputActions.Disable(); // Disable the input actions
        menuToggleAction.performed -= HandleMenuToggle; // Unregister the HandleMenuToggle method
        inputActions.General.CombatToggle.performed -= HandleCombatToggle;
        movementState?.Exit();
        combatState?.Exit();
    }

    private void HandleInputActions(GameState state)
    {
        switch (state)
        {
            case GameState.NormalMovement:
                inputActions.BasicMovement.Enable();
                inputActions.Gliding.Enable();
                inputActions.InMenu.Disable();
                inputActions.Combat.Disable();
                break;
            case GameState.Menu:
                inputActions.BasicMovement.Disable();
                inputActions.Gliding.Disable();
                inputActions.InMenu.Enable();
                inputActions.Combat.Disable();
                break;
            case GameState.Combat:
                inputActions.BasicMovement.Enable();
                inputActions.Gliding.Disable();
                inputActions.InMenu.Disable();
                inputActions.Combat.Enable();
                break;
        }
    }
}