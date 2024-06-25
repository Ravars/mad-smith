using System;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        private CharacterManager _characterManager;
        
        [Header("Position")] 
        [SyncVar] public Vector3 networkPosition;
        [SyncVar] public Quaternion networkRotation;
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("Animator")] 
        [SyncVar] public bool isMoving;

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }


        [Command] //ServerRpc
        public void NotifyTheServerOfActionAnimation(uint clientId, string animationId, bool applyRootMotion)
        {
            if (isServer)
            {
                PlayActionAnimationForAllClients(clientId, animationId, applyRootMotion);
            }
        }

        [ClientRpc]
        private void PlayActionAnimationForAllClients(uint clientId, string animationId, bool applyRootMotion)
        {
            PerformActionAnimationFromServer(animationId, applyRootMotion);
        }

        private void PerformActionAnimationFromServer(string animationId, bool applyRootMotion)
        {
            _characterManager.animator.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(animationId, 0.2f);
        }
        
    }
}