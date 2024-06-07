using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using MadSmith.Scripts.Interaction;
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
                ReleaseItem();
                GrabItem();
            }
            else
            {
                Debug.Log("Grab");
                GrabItem();
            }
        }

        private void ReleaseItem()
        {
            var item = _playerManager.playerInventoryManager.item;
            item.transform.SetParent(null);
            item.transform.SetPositionAndRotation(positionToReleaseItems.position, quaternion.identity);
            _playerManager.playerInventoryManager.item = null;
            item.SetStateCollider(true);
            item.SetStatePhysics(true);
        }

        private void GrabItem()
        {
            if (ReferenceEquals(lastTransformHit, null)) return;
            if (!lastTransformHit.TryGetComponent(out Item item)) return;
            item.transform.parent = rightHand;
            item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            item.SetStateCollider(false);
            item.SetStatePhysics(false);
            _playerManager.playerInventoryManager.item = item;
        }
        
        
        #region Tests with SphereCast
        [Range(0.1f, 1f)] public float sphereCastRadius;
        [Range(1f, 100f)] public float range = 1;
        [SerializeField] private LayerMask layerMask;
        public GameObject lastTransformHit = null;

        

        public override void FixedUpdate()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward * range, out hit, range, layerMask))
            {
                if (lastTransformHit != hit.transform.gameObject)
                {
                    lastTransformHit = hit.transform.gameObject;
                    Debug.Log("Get" + lastTransformHit);
                }
            }
            else
            {
                lastTransformHit = null;
            }
        }

        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        
            RaycastHit hit;
            Vector3 sphereCastOrigin = transform.position + transform.forward * (sphereCastRadius + 0.01f);
            if (Physics.SphereCast(sphereCastOrigin, sphereCastRadius, transform.forward * range, out hit, range, layerMask))
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