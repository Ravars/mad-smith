using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerInputManager _playerInputManager;
        private PlayerManager _playerManager;
        public bool isMoving;

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
            }
            else
            {
                isMoving = _playerManager.characterNetworkManager.isMoving;
                _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(isMoving);
            }
        }

        public void HandleAllLocomotion()
        {
            if (_playerManager.isPerformingAction) return;
            HandleGroundedMovement();
        }

        private void HandleGroundedMovement()
        {
            Vector3 movementDirection = new Vector3(_playerInputManager.MovingInputDirection.x, 0, _playerInputManager.MovingInputDirection.y);
            if (movementDirection.magnitude > 0.01f)
            {
                _playerInputManager.TargetRotation = Quaternion.LookRotation(movementDirection);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, _playerInputManager.TargetRotation, Time.deltaTime * _playerInputManager.SmoothRotation);
            movementDirection = movementDirection.normalized;
            _playerManager.characterController.SimpleMove(movementDirection * _playerInputManager.MoveSpeed);
            isMoving = _playerInputManager.MovingInputDirection.magnitude > 0.01f;
            _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(isMoving);
        }

        public void AttemptPerformDash()
        {
            if (_playerManager.isPerformingAction) return;
            
            if (_playerInputManager.MovingInputDirection.magnitude > 0.01f)
            {
                // _dashDirection = _playerInputManager.MovingInputDirection;
                // _dashDirection = transform.forward * _playerInputManager.MovingInputDirection.x;
                // _dashDirection += transform.right * _playerInputManager.MovingInputDirection.y;
                // _dashDirection.y = 0;
                _dashDirection = new Vector3(_playerInputManager.MovingInputDirection.x, 0, _playerInputManager.MovingInputDirection.y);
                // _dashDirection.Normalize();
                Quaternion playerRotation = Quaternion.LookRotation(_dashDirection);
                Debug.Log(playerRotation);
                _playerManager.transform.rotation = playerRotation;
                
                // Perform a dash to direction
                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Dash_forward", true);
            }   
            else
            {
                
                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Dash_forward", true);
                // Perform a dash forward
            }
        }

    }
}