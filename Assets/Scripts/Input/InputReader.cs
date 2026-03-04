using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, GameInput.IUIActions, GameInput.IDialogueActions
    {
        public event Action<E_InputMethod> InputControllerChangeEvent = delegate { };

        //Dialogue Events
        public event Action ContinueEvent = delegate { };
        public event Action SkipEvent = delegate { };

        private GameInput gameInput;

        void OnEnable()
        {
            if (gameInput == null)
            {
                gameInput = new GameInput();
            }

            gameInput.UI.SetCallbacks(this);
            gameInput.Dialogue.SetCallbacks(this);

            gameInput.Enable();

            InputSystem.onActionChange += HandleOnActionChange;
        }

        private void HandleOnActionChange(object arg1, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)arg1;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;

                if (lastDevice is Keyboard || lastDevice is Mouse)
                {
                    //Debug.Log("User is using Mouse/Keyboard");
                    InputControllerChangeEvent.Invoke(E_InputMethod.MouseKeyboard);
                }
                else if (lastDevice is Gamepad)
                {
                    switch (Gamepad.current)
                    {
                        case DualShockGamepad:
                            InputControllerChangeEvent.Invoke(E_InputMethod.Ps4);
                            break;
                        case XInputController:
                            InputControllerChangeEvent.Invoke(E_InputMethod.Xbox);
                            break;
                    }
                    InputControllerChangeEvent.Invoke(E_InputMethod.Gamepad);
                }
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
        public void OnCancel(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}


