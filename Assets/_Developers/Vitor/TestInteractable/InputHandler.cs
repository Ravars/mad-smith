using System;
using TestInteractable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Developers.Vitor.TestInteractable
{
    public class InputHandler : MonoBehaviour, InteractableInput.IPlayerActions
    {
        private InteractableInput _input;
        
        
        //Gameplay
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction MoveCanceledEvent = delegate { };
        public event UnityAction InteractEvent = delegate { };

        private void OnEnable()
        {
            if (_input == null)
            {
                _input = new InteractableInput();
                _input.Player.SetCallbacks(this);
                Disable();
                
            }
        }

        private void Awake()
        {
        }

        public void EnableGameplay()
        {
            _input.Player.Enable();
        }

        public void Disable()
        {
            _input.Disable();
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _input.Player.Move.ReadValue<Vector2>();
            return inputVector;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveEvent.Invoke(context.ReadValue<Vector2>());
            }else if (context.canceled)
            {
                MoveCanceledEvent.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                InteractEvent.Invoke();
            }
        }
    }
}