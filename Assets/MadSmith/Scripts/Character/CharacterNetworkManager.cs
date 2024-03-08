using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        [SyncVar] public Vector3 networkPosition;
        [SyncVar] public Quaternion networkRotation;
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("Animator")] 
        [SyncVar] public bool isMoving;
    }
}