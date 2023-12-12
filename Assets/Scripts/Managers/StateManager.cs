using UnityEngine;
using UnityEngine.InputSystem;

public class StateManager : MonoBehaviour
{
    // Enumeration for different game states
    private MovementState movementState;

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

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize the input actions
        inputActions = new PlayerInputs();

        // Register the escape key action
        inputActions.BasicMovement.Menu.performed += _ => ToggleMenu();

        // Initialize the game state to NormalMovement
        ChangeState(GameState.NormalMovement);

        var playerController = FindObjectOfType<PlayerController>(); // Assuming there is only one PlayerController in the scene.
        if (playerController != null)
        {
            movementState = new MovementState(playerController);
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
                // Enable menu inputs/actions
                // Setup menu UI
                // inputActions.Menu.Enable(); // Assuming you have a Menu action map
                break;

            case GameState.NormalMovement:
                Debug.Log("Entering Normal Movement State");
                inputActions.BasicMovement.Enable();
                movementState?.Enter();
                break;

            case GameState.Combat:
                Debug.Log("Entering Combat State");
                // Enable combat inputs/actions
                // Setup player for combat
                // inputActions.Combat.Enable(); // Assuming you have a Combat action map
                break;
        }
    }

    // Method to disable all inputs/actions
    private void DisableAllInputs()
    {
        Debug.Log("Disabling all inputs");
        // disable all input action maps
        inputActions.BasicMovement.Disable();
        // inputActions.Menu.Disable();
        // inputActions.Combat.Disable();
    }

    // Method to toggle the menu state
    private void ToggleMenu()
    {
        // Toggle state between Menu and NormalMovement
        if (currentState == GameState.NormalMovement)
        {
            ChangeState(GameState.Menu);
        }
        else if (currentState == GameState.Menu)
        {
            ChangeState(GameState.NormalMovement);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Escape key to toggle menu
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleMenu();
        }

        // Call Execute on the current state
        if (currentState == GameState.NormalMovement)
        {
            movementState?.Execute();
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
        movementState?.Exit();
    }
}
