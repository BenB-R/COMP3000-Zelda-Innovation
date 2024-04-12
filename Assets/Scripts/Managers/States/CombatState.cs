using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace fyp
{
    // CombatState
    public class CombatState : IState
    {
        private PlayerController playerController;
        private CameraController cameraController;
        private PlayerInputs inputActions;
        private Transform currentTarget; // The current enemy the player is locked onto

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

            // Enable combat controls
            inputActions.Combat.Enable();

            // Check if the player wants to lock onto a target
            if (playerController.WantsToLockOn())
            {
                playerController.ToggleLockOn();
                playerController.SetWantsToLockOn(false); // Reset the flag or method
            }

            // If there's a target, update camera and player rotation
            if (playerController.currentTarget != null)
            {
                cameraController.SetTarget(currentTarget);
            }

            // Bind combat action events for switching targets
            inputActions.Combat.SwitchLeft.performed += context => playerController.SwitchTarget(true);
            inputActions.Combat.SwitchRight.performed += context => playerController.SwitchTarget(false);
        }

        public void Execute()
        {
            Debug.Log("CombatState.Execute() called");
            Vector2 movementInput = inputActions.BasicMovement.Move.ReadValue<Vector2>();
            Vector3 movementVector = new Vector3(movementInput.x, 0, movementInput.y);
            playerController.CombatMovement(movementVector, true); // true indicates isInCombatMode
        }

        public void Exit()
        {
            Debug.Log("Exited Combat State");

            // Disable combat controls
            inputActions.Combat.Disable();

            // Unbind combat action events
            inputActions.Combat.SwitchLeft.performed -= SwitchTargetLeft;
            inputActions.Combat.SwitchRight.performed -= SwitchTargetRight;

            // Disable lock-on when exiting combat state
            playerController.ToggleLockOn();
        }

        private void SwitchTargetLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                playerController.SwitchTarget(true); // Switch to the left target
            }
        }

        private void SwitchTargetRight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                playerController.SwitchTarget(false); // Switch to the right target
            }
        }
    }
}
