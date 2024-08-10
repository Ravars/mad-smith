using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager _playerManager;
        
        public override void Awake()
        {
            base.Awake();
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

        // [Command]
        public void CastAttackHitBox()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward, meleeAttackRadius,
                meleeAttackLayerMask);
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("Collider: " + hitCollider.name);
                if (hitCollider.gameObject != gameObject)
                {
                    Debug.Log("hitCollider: " + hitCollider.name);
                    CmdApplyDamage(hitCollider.gameObject);
                }
            }
        }

        [Command]
        private void CmdApplyDamage(GameObject hitGameObject)
        {
            ClientApplyDamage(hitGameObject);
        }

        [ClientRpc]
        private void ClientApplyDamage(GameObject hitGameObject)
        {
            if(hitGameObject.TryGetComponent(out CharacterManager characterManager))
            {
                if (!characterManager.isDead)
                {
                    characterManager.characterCombatManager.TakeDamage(1);
                }
            }
        }
    }
}