using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager _playerManager;

        public override void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
        }
        
        
        public override void AttemptPerformAttack()
        {
            if (_playerManager.isPerformingAction) return;
            if (!_playerManager.isGrounded) return;
            
            
            
            _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Attacking", true, true,0.5f);
            // TODO: Verify if has weapon
            // TODO: Get current weapon (sword, staff)
        }
    }
}