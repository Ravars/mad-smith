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
        // public GameInput GameInput { get; set; }
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
                InputReader.MenuPauseEvent += InputReaderOnMenuPauseEvent;
            }
        }

        private void InputReaderOnMenuPauseEvent(int arg0)
        {
            if (arg0 == _playerManager.deviceId)
            {
                _playerManager.changeGameStatus.RaiseEvent(true);
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

        // private void GetAllInputs()
        // {
        //     
        // }

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
        private void InputReaderOnMoveEvent(Vector2 movingInputDirection, int deviceId)
        {
            if (deviceId != _playerManager.deviceId) return;
            MovingInputDirection = movingInputDirection;
        }
        private void InputReaderOnMoveCanceledEvent(int deviceId)
        {
            if (deviceId != _playerManager.deviceId) return;
            MovingInputDirection = Vector2.zero;
        }
        private void InputReaderOnDashEvent(int deviceId)
        {
            if (deviceId != _playerManager.deviceId) return;
            dashInput = true;
        }

        #endregion
    }
}