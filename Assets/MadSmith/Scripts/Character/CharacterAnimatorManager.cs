﻿using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private float _isMoving;
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        
        public void UpdateAnimatorMovementParameters(bool isMoving)
        {
            _characterManager.animator.SetBool(IsMoving, isMoving);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true)
        {
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(targetAnimation, 0.2f);
            // Used to stop starting animations in mid of another one
            _characterManager.isPerformingAction = isPerformingAction;
            Debug.Log(_characterManager.characterNetworkManager.netId);
            // _characterManager.characterNetworkManager.NotifyTheServerOfActionAnimation()
        }
    }
}