using System;
using Mirror;
using UnityEngine;

namespace _Developers.Vitor.TestInteractable
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NetworkTransformReliable))]
    [RequireComponent(typeof(InputHandler))]
    public class PlayerMovement : NetworkBehaviour
    {
        private Rigidbody _rb;
        private CharacterController _characterController;
        private InputHandler _inputHandler;
        
        private Vector3 _lastInteractDir;
        private BaseInteractable _selectedInteractable;
        [SerializeField] private LayerMask countersLayerMask;

        [SerializeField] private float movingSpeed = 3;
        
        private Vector2 _previousInput;
        private Quaternion _targetRotation;
        [SerializeField] private float smoothing = 5f; // Rotation
        [SerializeField] private float moveSpeed = 5f;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _characterController = GetComponent<CharacterController>();

            _characterController.enabled = false;
            _rb.isKinematic = true;

            _inputHandler = GetComponent<InputHandler>();
            this.enabled = false;
        }

        public override void OnStartAuthority()
        {
            _characterController.enabled = true;
            this.enabled = true;
            _inputHandler.EnableGameplay();
        }
        private void OnEnable()
        {
            _inputHandler.MoveEvent += SetMovement;
            _inputHandler.MoveCanceledEvent += ResetMovement;
            _inputHandler.InteractEvent += InputHandlerOnInteractEvent;
            // _inputHandler.DashEvent += DashOnPerformed;
        }

        private void OnDisable()
        {
            _inputHandler.MoveEvent -= SetMovement;
            _inputHandler.MoveCanceledEvent -= ResetMovement;
            _inputHandler.InteractEvent -= InputHandlerOnInteractEvent;
            // _inputReader.DashEvent -= DashOnPerformed;
        }

        private void Update()
        {
            if (!Application.isFocused) return;
            if (!_characterController.enabled) return;
            
            HandleMove();
            HandleInteractions();
        }
        private void SetMovement(Vector2 movement)
        {
            _previousInput = movement;
        }

        private void ResetMovement()
        {
            _previousInput = Vector2.zero;
        }
        private void InputHandlerOnInteractEvent()
        {
            if (_selectedInteractable != null)
            {
                Debug.Log("Has Interactable");
            }
        }
        private void HandleMove()
        {
            // Vector2 inputVector = _inputHandler.GetMovementVectorNormalized();
            //
            // Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
            //
            // _characterController.Move(moveDirection * (movingSpeed * Time.deltaTime));
            Vector3 movementDirection = new Vector3(_previousInput.x, 0, _previousInput.y);
            if (movementDirection.magnitude > 0.01f)
            {
                _targetRotation = Quaternion.LookRotation(movementDirection);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smoothing);
            movementDirection = movementDirection.normalized;
            _characterController.SimpleMove(movementDirection * moveSpeed);
        }

        private void HandleInteractions() {
            Vector2 inputVector = _inputHandler.GetMovementVectorNormalized();

            Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

            if (moveDir != Vector3.zero) {
                _lastInteractDir = moveDir;
            }

            float interactDistance = 2f;
            if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
                if (raycastHit.transform.TryGetComponent(out BaseInteractable baseCounter)) {
                    // Has ClearCounter
                    if (baseCounter != _selectedInteractable) {
                        SetSelectedCounter(baseCounter);
                    }
                } else {
                    SetSelectedCounter(null);

                }
            } else {
                SetSelectedCounter(null);
            }
        }
        private void SetSelectedCounter(BaseInteractable selectedCounter) {
            this._selectedInteractable = selectedCounter;
            
            // OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            //     selectedCounter = selectedCounter
            // });
        }
    }
}