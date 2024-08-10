using System;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        public float meleeAttackRadius;
        public LayerMask meleeAttackLayerMask;
        private CharacterManager _characterManager;
        public virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();

        }

        public virtual void AttemptPerformAttack()
        {
        }
        public void TakeDamage(int damage)
        {
            _characterManager.currentHealth = Math.Max(0, _characterManager.currentHealth - damage);
            Debug.Log("Damage: " + damage + " Id:" + netId);
            
        }
    }
}