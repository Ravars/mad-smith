using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Network
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        [SyncVar] public Vector3 networkPosition;
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
    }
}