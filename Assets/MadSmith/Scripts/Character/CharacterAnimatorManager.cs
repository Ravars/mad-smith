using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private float _isMoving;
        protected void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        
        public void UpdateAnimatorMovementParameters(bool isMoving)
        {
            _characterManager.animator.SetBool(IsMoving, isMoving);
        }
    }
}