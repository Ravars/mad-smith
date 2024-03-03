using System;
using MadSmith.Scripts.BaseClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MadSmith.Scripts.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : DescriptionBaseSo, GameInput.IGameplayActions
    {
        private GameInput _gameInput;
        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Gameplay.SetCallbacks(this);
            }
        }

        private void OnDisable()
        {
            DisableAllInputs();
        }

        public void DisableAllInputs()
        {
            _gameInput.Gameplay.Disable();
        }

        public void EnableGameplayInput()
        {
            DisableAllInputs();
            _gameInput.Gameplay.Enable();
        }

        public void SetState(bool state)
        {
            if (state)
            {
                _gameInput.Enable();
            }
            else
            {
                _gameInput.Disable();
            }
        }


        #region Unity Action Events
        //Menus
        public event UnityAction MenuPauseEvent = delegate { };
        
        //Gameplay
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction MoveCanceledEvent = delegate { };
        public event UnityAction InteractEvent = delegate { };
        public event UnityAction GrabEvent = delegate { };
        public event UnityAction DashEvent = delegate { };
        #endregion

        #region Gameplay Callbacks
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveEvent?.Invoke(context.ReadValue<Vector2>());
            }else if (context.canceled)
            {
                MoveCanceledEvent?.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                InteractEvent?.Invoke();
            }
        }

        public void OnGrab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                GrabEvent?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MenuPauseEvent?.Invoke();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                DashEvent?.Invoke();
            }
        }
        #endregion
    }
    
}
