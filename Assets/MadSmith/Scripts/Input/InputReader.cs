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
        public int deviceId = -1;
        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Gameplay.SetCallbacks(this);
                _gameInput.CouchJoin.SetCallbacks(this);
            }
        }

        public void SetDeviceId(int newDeviceId)
        {
            deviceId = newDeviceId;
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
        public event UnityAction MenuPauseEvent = delegate { };
        
        //Gameplay
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction MoveCanceledEvent = delegate { };
        public event UnityAction InteractEvent = delegate { };
        public event UnityAction GrabEvent = delegate { };
        public event UnityAction DashEvent = delegate { };
        
        //Couch Join
        public event UnityAction<int> JoinEvent = delegate { };
        #endregion

        #region Gameplay Callbacks
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed && context.control.device.deviceId == deviceId)
            {
                Debug.Log(context.control.device.deviceId);
                MoveEvent?.Invoke(context.ReadValue<Vector2>());
            }else if (context.canceled && context.control.device.deviceId == deviceId)
            {
                MoveCanceledEvent?.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && context.control.device.deviceId == deviceId)
            {
                InteractEvent?.Invoke();
            }
        }

        public void OnGrab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && context.control.device.deviceId == deviceId)
            {
                GrabEvent?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && context.control.device.deviceId == deviceId)
            {
                MenuPauseEvent?.Invoke();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed && context.control.device.deviceId == deviceId)
            {
                DashEvent?.Invoke();
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
        #endregion
    }
    
}
