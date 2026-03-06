using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

namespace DialogueSystem
{
    /// <summary>
    /// This class is responsible for delivering input events to other scripts. The benefit of 
    /// being a scriptable object is that any script that needs to know about input can ref the class 
    /// in the editor.
    /// </summary>

    //we only need one asset
    //[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, GameInput.IUIActions, GameInput.IPlayerActions, GameInput.IDialogueActions
    {
        public event Action<E_InputMethod> InputControllerChangeEvent = delegate { };

        //Dialogue Events
        public event Action ContinueEvent = delegate { };
        public event Action SkipEvent = delegate { };

        //Player Events
        public event Action<Vector2> MoveEvent = delegate { };
        public event Action InteractEvent = delegate { };
        private InputDevice currentDevice;
        private GameInput gameInput;

        void OnEnable()
        {
            if (gameInput == null)
            {
                gameInput = new GameInput();
            }

            gameInput.UI.SetCallbacks(this);
            gameInput.Dialogue.SetCallbacks(this);
            gameInput.Player.SetCallbacks(this);

            gameInput.Enable();

            InputSystem.onActionChange += HandleOnActionChange;
        }

        public void EnablePlayerInput() => gameInput.Player.Enable();
        public void DisablePlayerInput() => gameInput.Player.Disable();
        public void EnableDialogueInput() => gameInput.Dialogue.Enable();
        public void DisableDialogueInput() => gameInput.Dialogue.Disable();


        private void HandleOnActionChange(object arg1, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)arg1;
                var control = inputAction.activeControl;

                // This will print things like: "Gamepad:/leftStick/x" or "Gamepad:/buttonSouth"
                //Debug.Log($"Action: {inputAction.name} | Control: {control.path} | Value: {inputAction.ReadValueAsObject()}");
            }

            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)arg1;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;
                if (lastDevice == currentDevice) return;

                if (lastDevice is Keyboard || lastDevice is Mouse)
                {
                    Debug.Log("User is using Mouse/Keyboard");
                    InputControllerChangeEvent.Invoke(E_InputMethod.MouseKeyboard);
                }
                else if (lastDevice is Gamepad)
                {
                    switch (Gamepad.current)
                    {
                        case DualShockGamepad:
                            Debug.Log("playstation");
                            InputControllerChangeEvent.Invoke(E_InputMethod.PlayStation);
                            break;
                        case XInputController:
                            Debug.Log("xbox");
                            InputControllerChangeEvent.Invoke(E_InputMethod.Xbox);
                            break;
                        default:
                            InputControllerChangeEvent.Invoke(E_InputMethod.Gamepad);
                            break;
                    }
                }
                currentDevice = lastDevice.device;
            }
        }

        #region Dialogue
        public void OnContinue(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ContinueEvent.Invoke();
            }
        }

        public void OnSkip(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SkipEvent.Invoke();
            }
        }
        #endregion

        #region UI
        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
        }
        #endregion

        #region Player
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = Vector2.zero;
            if (context.performed)
            {
                input = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                input = Vector2.zero;
            }
            MoveEvent.Invoke(input.normalized);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InteractEvent.Invoke();
            }
        }
        #endregion
        void OnDisable()
        {
            gameInput.Disable();
            InputSystem.onActionChange -= HandleOnActionChange;

        }


    }
}


