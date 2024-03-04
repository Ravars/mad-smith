using System;
using _Developers.Vitor.TestCoop;
using MadSmith.Scripts.BaseClasses;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

namespace MadSmith.Scripts.Player
{
    public class PlayerManager : CharacterManager
    {
        public PlayerLocomotionManager playerLocomotionManager;
        public int playerNumber;
        public int deviceId = -1;
        public CoopManager coopManager;
        public TextMeshProUGUI deviceText;
        [field:SerializeField] public InputReader InputReader { get; private set; }
        
        public override void OnStartAuthority()
        {
            Debug.Log("OnStartAuthority" + deviceId + " +" + playerNumber);
            base.OnStartAuthority();
            SetInputReader(InputManager.Instance.GetInputReader(playerNumber));
            playerLocomotionManager.enabled = true;
            characterController.enabled = true; 
            if (deviceId != -1)
            {
                InputReader.SetDeviceId(deviceId);
                deviceText.text = (isOwned ? "Local" : "Remote") + " - " + (deviceId == 1 ? "KeyBoard" : "Controller");
                InputReader.EnableGameplayInput();
            }
            else
            {
                InputReader.EnableCouchInput();
                InputReader.JoinEvent += InputReaderOnJoinEvent;
            }
        }

        private void InputReaderOnJoinEvent(int usedDeviceId)
        {
            if (InputReader.deviceId != usedDeviceId && InputReader.deviceId == -1 && playerNumber == 0)
            {
                InputReader.SetDeviceId(usedDeviceId);
                deviceId = usedDeviceId;
                InputReader.EnableGameplayInput();
                deviceText.text = (isOwned ? "Local" : "Remote") + " - " + (deviceId == 1 ? "KeyBoard" : "Controller");
            }

            if (InputReader.deviceId != usedDeviceId && coopManager.IsNewDevice(usedDeviceId))
            {
                deviceId = usedDeviceId;
                coopManager.CmdCallAddPlayer(playerNumber + 1, usedDeviceId);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerLocomotionManager.enabled = false;
        }

        protected override void Update()
        {
            base.Update();
            if (!isOwned) return;
            playerLocomotionManager.HandleAllLocomotion();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (enabled && InputReader != null)
            {
                InputReader.SetState(hasFocus);
            }
        }

        private void SetInputReader(InputReader inputReader)
        {
            InputReader = inputReader;
        }
    }
}