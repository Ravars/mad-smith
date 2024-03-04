using System;
using MadSmith.Scripts.Network;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.BaseClasses
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;
        public CharacterNetworkManager characterNetworkManager;
        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterController.enabled = false;
        }

        protected virtual void Update()
        {
            if (isOwned) // Test if this is right or should I use IsOwned   
            {
                characterNetworkManager.networkPosition = transform.position;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetworkManager.networkPosition,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);
            }
        }
    }
}