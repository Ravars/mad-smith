using MadSmith.Scripts.Interaction;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerInteractionManager : CharacterInteractionManager
    {
        private PlayerManager _playerManager;
        
        [Header("Interaction Settings")]
        [Range(0.1f, 1f)] public float sphereCastRadius = 0.1f;
        [Range(1f, 100f)] public float range = 1;
        [SerializeField] private LayerMask layerMask;
        private GameObject _lastTransformHit = null;
        
        [Header("Slots Holder")]
        [ShowInInspector] private SlotHolder _rightHand;
        // [ShowInInspector] private SlotHolder _doubleHand;
        
        
        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            SlotHolder[] itemSlotHolder = GetComponentsInChildren<SlotHolder>();
            foreach (var slotHolder in itemSlotHolder) //TODO: Maybe change to a Switch later
            {
                if (slotHolder.itemSlotHolderCategory == ItemSlotHolderCategory.Hand)
                {
                    _rightHand = slotHolder;
                }
                // else if (slotHolder.itemSlotHolderCategory == ItemSlotHolderCategory.DoubleHand)
                // {
                //     _doubleHand = slotHolder;
                // }
                
            }
        }

        public void AttemptPerformGrab()
        {
            //TODO: test this two lines
            if (_playerManager.isPerformingAction) return;
            if (!_playerManager.isGrounded) return;

            bool canGetAnotherItem = false;
            bool wasHoldingItem = false;
            if (_playerManager.playerInventoryManager.IsHoldingItem())
            {
                wasHoldingItem = true;
                if (!ReferenceEquals(_lastTransformHit, null))
                {
                    if (_lastTransformHit.TryGetComponent<Table>(out Table table))
                    {
                        Debug.Log("Attempt place");
                        _playerManager.playerNetworkManager.CmdAttemptPlaceItem(_playerManager.playerInventoryManager.GetItem(), table);
                    }
                    
                }
                else
                {
                    Debug.Log("Drop");
                    _playerManager.playerNetworkManager.CmdReleaseItem();
                    canGetAnotherItem = true;
                }
                
            }
            
            if (_lastTransformHit != null && (canGetAnotherItem || !wasHoldingItem))
            {
                Debug.Log("Grab else: " + _lastTransformHit.name);
                if(_lastTransformHit.CompareTag("Item"))
                { 
                    Debug.Log("item");
                    _playerManager.playerNetworkManager.CmdAttemptPickupItem(_lastTransformHit);
                }
                else if (_lastTransformHit.CompareTag("Table") && _lastTransformHit.TryGetComponent<Table>(out Table table))
                {
                    Debug.Log("Table");
                    _playerManager.playerNetworkManager.CmdAttemptPickupTableItem(table);
                }
            }
        }

        
        
        #region Tests with SphereCast
        public void HandleHighlight()
        {
            if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward * range, out var hit, range, layerMask))
            {
                if (_lastTransformHit != hit.transform.gameObject)
                {
                    _lastTransformHit = hit.transform.gameObject;
                    if (hit.transform.gameObject.TryGetComponent(out Interactable interactable))
                    {
                        interactable.SetStateHighlight(true);
                    }
                }
            }
            else
            {
                if (_lastTransformHit != null && _lastTransformHit.TryGetComponent(out Interactable interactable))
                {
                    interactable.SetStateHighlight(false);
                }
                _lastTransformHit = null;
            }
        }

        public void UpdateMesh()
        {
            SlotHolder slotHolder = DecideSlotHolder();
            
            if (_playerManager.playerNetworkManager.myItem != null && _playerManager.playerNetworkManager.myItem.TryGetComponent(out Item item))
            {
                slotHolder.meshRenderer.material = item.baseItem.material;
                slotHolder.meshFilter.mesh = item.baseItem.mesh;
            }
            else
            {
                //NOTE: Use the last used or clear them all
                slotHolder.meshRenderer.material = null;
                slotHolder.meshFilter.mesh = null;
            }
        }

        private SlotHolder DecideSlotHolder()
        {
            //TODO: Decide witch slot to use based on item used, rightHand or doubleHand
            return _rightHand;
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