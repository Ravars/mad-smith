using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private static readonly int IsMovingAnimation = Animator.StringToHash("IsMoving");
        private float _isMoving;
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        
        public void UpdateAnimatorMovementParameters(bool isMoving)
        {
            _characterManager.animator.SetBool(IsMovingAnimation, isMoving);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, float normalizedTransitionDuration = 0.2f)
        {
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(targetAnimation, normalizedTransitionDuration);
            // Used to stop starting animations in mid of another one
            _characterManager.isPerformingAction = isPerformingAction;
            // Debug.Log(_characterManager.characterNetworkManager.netId);
            _characterManager.characterNetworkManager.NotifyTheServerOfActionAnimation(_characterManager.characterNetworkManager.netId, targetAnimation, applyRootMotion);
        }
    }
}