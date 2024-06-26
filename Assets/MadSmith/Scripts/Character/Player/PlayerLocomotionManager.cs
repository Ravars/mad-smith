using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerInputManager _playerInputManager;
        private PlayerManager _playerManager;
        public bool isMoving;
        public bool isAiming;
        

        [Header("Dash")] 
        private Vector3 _dashDirection;
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
            _playerInputManager = GetComponent<PlayerInputManager>();
        }

        protected override void Update()
        {
            base.Update();
            if (_playerManager.isOwned)
            {
                _playerManager.characterNetworkManager.isMoving = isMoving;
                _playerManager.characterNetworkManager.isAiming = isAiming;
            }
            else
            {
                isMoving = _playerManager.characterNetworkManager.isMoving;
                isAiming = _playerManager.characterNetworkManager.isAiming;
                _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(isMoving);
                _playerManager.playerAnimatorManager.UpdateAnimatorAimingParameters(isAiming);
            }
        }

        public void HandleAllLocomotion()
        {
            // if (_playerManager.isPerformingAction) return;
            HandleGroundedMovement();
            HandleRotation();
        }

        private void HandleGroundedMovement()
        {
            if (!_playerManager.canMove) return;
            Vector3 movementDirection = new Vector3(_playerInputManager.MovingInputDirection.x, 0, _playerInputManager.MovingInputDirection.y);
            movementDirection.Normalize();
            _playerManager.characterController.Move(movementDirection * (MoveSpeed * Time.deltaTime));
            isMoving = _playerInputManager.MovingInputDirection.magnitude > 0.01f;
            _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(isMoving);
        }

        private void HandleRotation()
        {
            if (!_playerManager.canRotate) return;
            Vector3 movementDirection = new Vector3(_playerInputManager.MovingInputDirection.x, 0, _playerInputManager.MovingInputDirection.y);
            if (movementDirection.magnitude > 0.01f)
            {
                _playerInputManager.TargetRotation = Quaternion.LookRotation(movementDirection);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, _playerInputManager.TargetRotation, Time.deltaTime * SmoothRotation);
            
        }

        public void AttemptPerformDash()
        {
            if (_playerManager.isPerformingAction) return;

            if (!_playerManager.isGrounded) return;
            
            if (_playerInputManager.MovingInputDirection.magnitude > 0.01f)
            {
                Vector3 animationRotation = new Vector3(0f, 27.56f, 0f); //offset due to Mixamo rotation
                _dashDirection = new Vector3(_playerInputManager.MovingInputDirection.x, 0, _playerInputManager.MovingInputDirection.y);
                Vector3 adjustedDashDirection = Quaternion.Euler(animationRotation) * _dashDirection;
                Quaternion playerRotation = Quaternion.LookRotation(adjustedDashDirection);
                
                _playerManager.transform.rotation = playerRotation;
                
                // Perform a dash to direction
                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Dash_forward", true);
            }   
            else
            {
                Vector3 animationRotation = new Vector3(0f, 27.56f, 0f); //offset due to Mixamo rotation
                _dashDirection = transform.forward;
                Vector3 adjustedDashDirection = Quaternion.Euler(animationRotation) * _dashDirection;
                Quaternion playerRotation = Quaternion.LookRotation(adjustedDashDirection);
                
                _playerManager.transform.rotation = playerRotation;
                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Dash_forward", true);
                // Perform a dash forward
            }
        }
        public void AttemptPerformAiming()
        {
            if (_playerManager.isPerformingAction) return;
            if (!_playerManager.isGrounded) return;
            if (isAiming) return;
            if (!_playerManager.playerInventoryManager.IsHoldingItem()) return;
            
            isAiming = true;
            //TODO: Start aiming visual effect
            Debug.Log("Aiming VFX");
            _playerManager.playerAnimatorManager.UpdateAnimatorAimingParameters(isAiming);
            _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Throw", true, false, 0.2f, false, true);
        }

        public void AttemptThrow()
        {
            if (!isAiming) return;
            if (!_playerManager.playerInventoryManager.IsHoldingItem()) return;
            Debug.Log("Throw item");
            isAiming = false;
            _playerManager.playerAnimatorManager.PlayTargetActionAnimation("ThrowRelease", true);
            _playerManager.playerAnimatorManager.UpdateAnimatorAimingParameters(false);
            _playerManager.playerNetworkManager.CmdThrowItem();
        }

    }
}