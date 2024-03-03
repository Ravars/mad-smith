using System;
using MadSmith.Scripts.BaseClasses;
using UnityEngine;

namespace MadSmith.Scripts.Player
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager _playerManager;
        
        //Moving
        private Vector2 _movingInputDirection;
        private Quaternion _targetRotation;
        [SerializeField] private float smoothRotation = 5f;
        [SerializeField] private float moveSpeed = 5f;
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }
        private void OnEnable()
        {
            
            _playerManager.InputReader.MoveEvent += InputReaderOnMoveEvent;
            _playerManager.InputReader.MoveCanceledEvent += InputReaderOnMoveCanceledEvent;
        }

        private void OnDisable()
        {
            _playerManager.InputReader.MoveEvent -= InputReaderOnMoveEvent;
        }

        public void HandleAllLocomotion()
        {
            HandleGroundedMovement();
        }

        private void HandleGroundedMovement()
        {
            Vector3 movementDirection = new Vector3(_movingInputDirection.x, 0, _movingInputDirection.y);
            if (movementDirection.magnitude > 0.01f)
            {
                _targetRotation = Quaternion.LookRotation(movementDirection);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smoothRotation);
            movementDirection = movementDirection.normalized;
            _playerManager.characterController.SimpleMove(movementDirection * moveSpeed);
        }

        #region Events Subscriptions
        private void InputReaderOnMoveEvent(Vector2 movingInputDirection)
        {
            _movingInputDirection = movingInputDirection;
        }
        private void InputReaderOnMoveCanceledEvent()
        {
            _movingInputDirection = Vector2.zero;
        }

        #endregion
    }
}