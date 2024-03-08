using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerInputManager _playerInputManager;
        private PlayerManager _playerManager;
        public bool isMoving;
        
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


    }
}