using System;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterCombatManager : MonoBehaviour
    {
        public int currentHealth;
        public int maxHealth;

        public virtual void Awake()
        {
        }

        public virtual void AttemptPerformAttack()
        {
        }
    }
}