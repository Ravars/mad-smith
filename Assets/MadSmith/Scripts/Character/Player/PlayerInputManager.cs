using System;
using MadSmith.Scripts.Input;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerInputManager : CharacterInputManger
    {
        private PlayerManager _playerManager;
        
        [Header("Player Actions")]
        [SerializeField] private bool dashInput = false;
        [field:SerializeField] public InputReader InputReader { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }


        private void OnEnable()
        {
            if(InputReader != null)
            {
                InputReader.MoveEvent += InputReaderOnMoveEvent;
                InputReader.MoveCanceledEvent += InputReaderOnMoveCanceledEvent;
                InputReader.DashEvent += InputReaderOnDashEvent;
            }
        }
        private void OnDisable()
        {
            if (InputReader != null)
            {
                InputReader.MoveEvent -= InputReaderOnMoveEvent;
                InputReader.MoveCanceledEvent -= InputReaderOnMoveCanceledEvent;
                InputReader.DashEvent -= InputReaderOnDashEvent;
            }
        }
        public void SetInputReader(InputReader inputReader)
        {
            InputReader = inputReader;
        }

        protected override void Update()
        {
            base.Update();
            HandleDashInput();
        }

        private void HandleDashInput()
        {
            if (dashInput)
            {
                dashInput = false;
                
                //Perform a dash
                _playerManager.playerLocomotionManager.AttemptPerformDash();
            }
        }

        #region Events Subscriptions
        private void InputReaderOnMoveEvent(Vector2 movingInputDirection)
        {
            MovingInputDirection = movingInputDirection;
        }
        private void InputReaderOnMoveCanceledEvent()
        {
            MovingInputDirection = Vector2.zero;
        }
        private void InputReaderOnDashEvent()
        {
            dashInput = true;
        }

        #endregion
    }
}