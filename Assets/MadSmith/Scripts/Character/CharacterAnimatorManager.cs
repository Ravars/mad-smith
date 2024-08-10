using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterAnimatorManager : NetworkBehaviour
    {
        private CharacterManager _characterManager;
        private static readonly int IsMovingAnimation = Animator.StringToHash("IsMoving");
        private static readonly int IsAimingAnimation = Animator.StringToHash("IsAiming");
        private float _isMoving;
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        
        public void UpdateAnimatorMovementParameters(bool isMoving)
        {
            _characterManager.animator.SetBool(IsMovingAnimation, isMoving);
        }

        public void UpdateAnimatorAimingParameters(bool isAiming)
        {
            _characterManager.animator.SetBool(IsAimingAnimation, isAiming);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            float normalizedTransitionDuration = 0.2f,
            bool canMove = false,
            bool canRotate = false)
        {
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(targetAnimation, normalizedTransitionDuration);
            // Used to stop starting animations in mid of another one
            _characterManager.isPerformingAction = isPerformingAction;
            _characterManager.characterNetworkManager.CmdNotifyTheServerOfActionAnimation(_characterManager.characterNetworkManager.netId, targetAnimation, applyRootMotion);
            _characterManager.canMove = canMove;
            _characterManager.canRotate = canRotate;
        }

        public virtual void AnimationAttackEvent()
        {
            
        }
    }
}