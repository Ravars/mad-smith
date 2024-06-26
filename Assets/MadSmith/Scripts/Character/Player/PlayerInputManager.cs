using MadSmith.Scripts.Input;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerInputManager : CharacterInputManger
    {
        private PlayerManager _playerManager;
        
        [Header("Player Actions")]
        [SerializeField] private bool dashInput = false;
        [SerializeField] private bool grabInput = false;
        [SerializeField] private bool attackInput = false;
        [SerializeField] private bool aimInput = false;
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
                InputReader.GrabEvent += InputReaderOnGrabEvent;
                InputReader.AttackEvent += InputReaderOnAttackEvent;
                InputReader.AimEvent += InputReaderOnAimEvent;
                InputReader.AimCanceledEvent += InputReaderOnAimCanceledEvent;
            }
        }
        private void InputReaderOnMenuPauseEvent(int deviceId)
        {
            if (deviceId == _playerManager.deviceId)
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
                InputReader.MenuPauseEvent -= InputReaderOnMenuPauseEvent;
                InputReader.GrabEvent -= InputReaderOnGrabEvent;
                InputReader.AttackEvent -= InputReaderOnAttackEvent;
                InputReader.AimEvent -= InputReaderOnAimEvent;
                InputReader.AimCanceledEvent -= InputReaderOnAimCanceledEvent;
            }
        }
        protected override void Update()
        {
            base.Update();
            HandleDashInput();
            HandleGrabInput();
            HandleAttackInput();
            HandleAimingInput();
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

        private void HandleGrabInput()
        {
            if (grabInput)
            {
                grabInput = false;
                
                //Perform a grab
                _playerManager.playerInteractionManager.AttemptPerformGrab();
            }
        }

        private void HandleAttackInput()
        {

            if (attackInput && _playerManager.playerLocomotionManager.isAiming)
            {
                attackInput = false;
                _playerManager.playerLocomotionManager.AttemptThrow();
            } else if (attackInput)
            {
                attackInput = false;
                
                //Perform a attack
                _playerManager.playerCombatManager.AttemptPerformAttack();
            }
        }

        private void HandleAimingInput()
        {
            if (aimInput)
            {
                aimInput = false;
                _playerManager.playerLocomotionManager.AttemptPerformAiming();
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
        private void InputReaderOnGrabEvent(int deviceId)
        {
            if (deviceId != _playerManager.deviceId) return;
            grabInput = true;
        }
        private void InputReaderOnAttackEvent(int deviceId)
        {
            if (deviceId != _playerManager.deviceId) return;
            attackInput = true;
        }

        private void InputReaderOnAimEvent(int deviceId)
        {
            if (deviceId != _playerManager.deviceId) return;
            aimInput = true;
        }
        private void InputReaderOnAimCanceledEvent(int deviceId)
        {
            if (deviceId != _playerManager.deviceId) return;
            aimInput = false;
            _playerManager.playerLocomotionManager.isAiming = false;
            _playerManager.playerAnimatorManager.UpdateAnimatorAimingParameters(false);
        }

        #endregion
    }
}