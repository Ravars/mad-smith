using System;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerInteractionManager : CharacterInteractionManager
    {
        private PlayerManager _playerManager;

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
        }

        public void AttemptPerformGrab()
        {
            Debug.Log("Grab");
        }
        public void AttemptPerformAttack()
        {
            if (_playerManager.isPerformingAction) return;
            if (!_playerManager.isGrounded) return;
            
            _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Attacking", true, true,0.5f);
            // Verify if has weapon
            // Get current weapon (sword, staff)
            
            Debug.Log("Attack");
        }
        
    }
}