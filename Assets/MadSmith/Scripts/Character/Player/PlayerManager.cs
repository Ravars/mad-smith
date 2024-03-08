using _Developers.Vitor.TestCoop;
using MadSmith.Scripts.Managers;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerInputManager playerInputManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        
        [Header("Coop System")]
        public int playerNumber;
        public int deviceId = -1;
        public CoopManager coopManager;
        
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            playerInputManager.SetInputReader(InputManager.Instance.GetInputReader(playerNumber));
            playerLocomotionManager.enabled = true;
            characterController.enabled = true; 
            playerInputManager.enabled = true;
            
            if (deviceId != -1)
            {
                playerInputManager.InputReader.SetDeviceId(deviceId);
                playerInputManager.InputReader.EnableGameplayInput();
            }
            else
            {
                playerInputManager.InputReader.EnableCouchInput();
                playerInputManager.InputReader.JoinEvent += InputReaderOnJoinEvent;
            }
        }

        private void InputReaderOnJoinEvent(int usedDeviceId)
        {
            if (playerInputManager.InputReader.deviceId != usedDeviceId && playerInputManager.InputReader.deviceId == -1 && playerNumber == 0)
            {
                playerInputManager.InputReader.SetDeviceId(usedDeviceId);
                deviceId = usedDeviceId;
                playerInputManager.InputReader.EnableGameplayInput();
            }

            if (playerInputManager.InputReader.deviceId != usedDeviceId && coopManager.IsNewDevice(usedDeviceId))
            {
                deviceId = usedDeviceId;
                coopManager.CmdCallAddPlayer(playerNumber + 1, usedDeviceId);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerInputManager = GetComponent<PlayerInputManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            
            // playerLocomotionManager.enabled = false;
            playerInputManager.enabled = false;
        }

        protected override void Update()
        {
            base.Update();
            if (!isOwned) return;
            playerLocomotionManager.HandleAllLocomotion();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (enabled && playerInputManager.InputReader != null)
            {
                playerInputManager.InputReader.SetState(hasFocus);
            }
        }
    }
}