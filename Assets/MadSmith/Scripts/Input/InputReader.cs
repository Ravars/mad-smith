using System;
using MadSmith.Scripts.BaseClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MadSmith.Scripts.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : DescriptionBaseSo, GameInput.IGameplayActions, GameInput.ICouchJoinActions
    {
        private GameInput _gameInput;
        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Gameplay.SetCallbacks(this);
                _gameInput.CouchJoin.SetCallbacks(this);
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
        public void EnableCouchInput()
        {
            DisableAllInputs();
            _gameInput.CouchJoin.Enable();
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
        public event UnityAction<int> MenuPauseEvent = delegate { };
        
        //Gameplay
        public event UnityAction<Vector2, int> MoveEvent = delegate { };
        public event UnityAction<int> MoveCanceledEvent = delegate { };
        public event UnityAction<int> InteractEvent = delegate { };
        public event UnityAction<int> GrabEvent = delegate { };
        public event UnityAction<int> DashEvent = delegate { };
        
        //Couch Join
        public event UnityAction<int> JoinEvent = delegate { };
        #endregion

        #region Gameplay Callbacks
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveEvent?.Invoke(context.ReadValue<Vector2>(), context.control.device.deviceId);
            }else if (context.canceled)
            {
                MoveCanceledEvent?.Invoke(context.control.device.deviceId);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InteractEvent?.Invoke(context.control.device.deviceId);
            }
        }

        public void OnGrab(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GrabEvent?.Invoke(context.control.device.deviceId);
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MenuPauseEvent?.Invoke(context.control.device.deviceId);
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DashEvent?.Invoke(context.control.device.deviceId);
            }
        }
        #endregion

        #region Join Couch
        public void OnJoin(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JoinEvent?.Invoke(context.control.device.deviceId);
            }
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MenuPauseEvent?.Invoke(context.control.device.deviceId);
            }
        }

        #endregion
    }
    
}
