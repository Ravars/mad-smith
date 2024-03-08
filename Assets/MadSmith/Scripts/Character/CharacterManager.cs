using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterController.enabled = false;
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
    }
}