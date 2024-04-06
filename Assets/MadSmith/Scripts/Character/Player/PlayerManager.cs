using MadSmith.Scripts.Events.ScriptableObjects;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerInputManager playerInputManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        public BoolEventChannelSo settingsPanelState;
        public BoolEventChannelSo changeGameStatus;
        
        [Header("Coop System")]
        [SyncVar] public int deviceId = -1;
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            Debug.Log("Start authority manager");
            // enabled = true;
            playerLocomotionManager.enabled = true;
            characterController.enabled = true; 
            playerInputManager.enabled = true;
            playerInputManager.InputReader.EnableGameplayInput();
            settingsPanelState.OnEventRaised += SettingsPanelStateOnEventRaised;
        }

        private void SettingsPanelStateOnEventRaised(bool value)
        {
            if (value)
            {
                playerInputManager.InputReader.EnableMenuInput();
            }
            else
            {
                playerInputManager.InputReader.EnableGameplayInput();
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
            // if (enabled && playerInputManager.InputReader != null)
            // {
            //     playerInputManager.InputReader.SetState(hasFocus);
            // }
        }
    }
}