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
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;

        
        [Header("Flags - Client")] 
        public bool isPerformingAction = false;
        public bool isGrounded = true;
        public bool applyRootMotion = false;
        public bool canMove = false;
        public bool canRotate = false;
        
        [Header("Character Stats - Synced")]
        [SyncVar] public bool isDead;
        [SyncVar(hook = nameof(HandleHealthChange))] public int currentHealth;
        [SyncVar] public int maxHealth = 2;
        
        private static readonly int IsGroundedAnimation = Animator.StringToHash("IsGrounded");

        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterController.enabled = false;
        }
        protected virtual void Init()
        {
            currentHealth = maxHealth;
            isDead = false;
        }
        
        protected virtual void Update()
        {
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
        protected virtual void HandleHealthChange(int oldState, int newState)
        {
            if (newState <= 0 && !isDead)
            {
                RpcProcessDeath();
                Invoke(nameof(InvokeRevive), 5);
            }

            if (newState > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        private void InvokeRevive()
        {
            RpcRevive();
        }
        
        [ClientRpc]
        public virtual void RpcProcessDeath()
        {
            if (isOwned)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Death", true);
            }

            if (isServer)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        
        [ClientRpc]
        public virtual void RpcRevive()
        {
            if (isOwned)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Empty", false);
            }

            if (isServer)
            {
                currentHealth = maxHealth;
                isDead = false;
            }
        }
    }
}