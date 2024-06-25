using MadSmith.Scripts.Interaction;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerInteractionManager : CharacterInteractionManager
    {
        private PlayerManager _playerManager;
        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
        }

        public void AttemptPerformGrab()
        {
            if (_playerManager.playerInventoryManager.IsHoldingItem())
            {
                Debug.Log("Drop");
                _playerManager.playerNetworkManager.CmdReleaseItem();
            }
            
            if (lastTransformHit != null)
            {
                Debug.Log("Grab else: " + lastTransformHit.name);
                _playerManager.playerNetworkManager.CmdAttemptPickupItem(lastTransformHit);
            }
        }
        
        #region Tests with SphereCast
        [Range(0.1f, 1f)] public float sphereCastRadius;
        [Range(1f, 100f)] public float range = 1;
        [SerializeField] private LayerMask layerMask;
        public GameObject lastTransformHit = null;

        

        public void HandleInteraction()
        {
            if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward * range, out var hit, range, layerMask))
            {
                if (lastTransformHit != hit.transform.gameObject)
                {
                    lastTransformHit = hit.transform.gameObject;
                    if (hit.transform.gameObject.TryGetComponent(out Item item))
                    {
                        item.SetStateHighlight(true);
                    }
                }
            }
            else
            {
                if (lastTransformHit != null && lastTransformHit.TryGetComponent(out Item item))
                {
                    item.SetStateHighlight(false);
                }
                lastTransformHit = null;
            }
        }

        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);

            Vector3 sphereCastOrigin = transform.position + transform.forward * (sphereCastRadius + 0.01f);
            if (Physics.SphereCast(sphereCastOrigin, sphereCastRadius, transform.forward * range, out var hit, range, layerMask))
            {
                Gizmos.color = Color.green;
                Vector3 sphereCastMidpoint = transform.position + (transform.forward * hit.distance);
                Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
                Gizmos.DrawSphere(hit.point, 0.1f);
                Debug.DrawLine(transform.position, sphereCastMidpoint, Color.green);
            }
            else
            {
                Gizmos.color = Color.red;
                Vector3 sphereCastMidpoint = transform.position + (transform.forward * (range-sphereCastRadius));
                Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
                Debug.DrawLine(transform.position, sphereCastMidpoint, Color.red);
            }
        }
        #endregion
    }
}