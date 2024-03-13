using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        private PlayerManager _playerManager;

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()
        {
            if (_playerManager.applyRootMotion)
            {
                Vector3 velocity = _playerManager.animator.deltaPosition;
                _playerManager.characterController.Move(velocity);
                _playerManager.transform.rotation *= _playerManager.animator.deltaRotation;
            }
        }
    }
}