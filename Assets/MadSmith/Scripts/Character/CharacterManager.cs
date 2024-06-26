using System;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;

        
        [Header("Flags")] 
        public bool isPerformingAction = false;
        public bool isGrounded = true;
        public bool applyRootMotion = false;
        public bool canMove = false;
        public bool canRotate = false;
        
        private static readonly int IsGroundedAnimation = Animator.StringToHash("IsGrounded");

        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterController.enabled = false;
        }

        protected virtual void Update()
        {
            
            // animator.SetBool(IsGroundedAnimation, isGrounded);
            if (isOwned)
            {
                characterNetworkManager.networkPosition = transform.position;
                characterNetworkManager.networkRotation = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetworkManager.networkPosition,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);
                
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    characterNetworkManager.networkRotation,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void FixedUpdate()
        {
        }
    }
}