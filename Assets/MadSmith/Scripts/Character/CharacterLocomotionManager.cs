using System;
using MadSmith.Scripts.Character.Player;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        private PlayerManager _character;

        [Header("Ground Check")]
        [SerializeField] private float gravityForce = -5.55f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckSphereRadius = 0.1f;
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20;
        [SerializeField] protected float fallStartYVelocity = -5;
        protected bool FallingVelocityHasBeenSet = false;
        
        protected float InAirTimer = 0;
        private static readonly int AirTimerAnimation = Animator.StringToHash("InAirTimer");


        protected virtual void Awake()
        {
            _character = GetComponent<PlayerManager>();
        }

        protected virtual void Update()
        {
            if (!_character.isOwned) return;
            HandleGroundCheck();

            if (_character.isGrounded)
            {
                InAirTimer = 0;
                FallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
            else
            {
                if (!FallingVelocityHasBeenSet)
                {
                    FallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                InAirTimer += Time.deltaTime;
                // _character.animator.SetFloat(AirTimerAnimation, InAirTimer);
                
                yVelocity.y = Mathf.Max(yVelocity.y + gravityForce * Time.deltaTime, groundedYVelocity);
            }
            _character.characterController.Move(yVelocity * Time.deltaTime);
        }
        
        private void HandleGroundCheck()
        {
            _character.isGrounded = Physics.CheckSphere(_character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, groundCheckSphereRadius);
        }
    }
}