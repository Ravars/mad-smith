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
            // if (GameInput == null)
            // {
            //     GameInput = new GameInput();
            //     GameInput.Enable();
            //     GameInput.Gameplay.Enable();
            // }
        }


        private void OnEnable()
        {
            if(InputReader != null)
            {
                // GameInput.Gameplay.Dash.performed += context => dashInput = true;
                // GameInput.Gameplay.Move.performed += context => Debug.Log(context.ReadValue<Vector2>());
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