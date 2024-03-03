using System;
using MadSmith.Scripts.BaseClasses;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

namespace MadSmith.Scripts.Player
{
    public class PlayerManager : CharacterManager
    {
        public PlayerLocomotionManager playerLocomotionManager;
        public int playerNumber;
        [field:SerializeField] public InputReader InputReader { get; private set; }
        
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            SetInputReader(InputManager.Instance.GetInputReader(playerNumber));
            playerLocomotionManager.enabled = true;
            InputReader.EnableGameplayInput();
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
            playerLocomotionManager.HandleAllLocomotion();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (enabled)
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