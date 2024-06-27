using System;
using MadSmith.Scripts.Interaction;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        
        private PlayerManager _playerManager;
        [Header("Inventory")]
        [SyncVar] public GameObject myItem;
        // [SyncVar] public Interactable CurrentInteractable;

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        public void Init()
        {
            _playerManager.playerInteractionManager.UpdateMesh();
        }
        [Command]
        public void CmdAttemptPickupItem(GameObject itemGameObject)
        {
            if (itemGameObject == null) return;
            if (!itemGameObject.TryGetComponent(out Item item) || !item.IsAvailable()) return;
            
            item.SetAvailable(false);
            RpcFakeAttachItem(itemGameObject);
        }

        [Command(requiresAuthority = false)]
        public void CmdAttemptPickupThrownItem(GameObject itemGameObject)
        {
            if (itemGameObject == null) return;
            if (_playerManager.playerInventoryManager.IsHoldingItem()) return;
            if (!itemGameObject.TryGetComponent(out Item item) || !item.IsAvailable()) return;
            
            item.SetAvailable(false);
            RpcFakeAttachItem(itemGameObject);
        }

        [Command]
        public void CmdReleaseItem()
        {
            Item item = _playerManager.playerNetworkManager.myItem.GetComponent<Item>();
            item.SetPosition(_playerManager.playerInteractionManager.positionToReleaseItems.position);
            item.SetRotation(Quaternion.identity);
            item.SetAvailable(true);
            RpcFakeDetachItem();
        }

        [ClientRpc]
        private void RpcFakeAttachItem(GameObject itemGameObject)
        {   
            _playerManager.playerNetworkManager.myItem = itemGameObject;
            _playerManager.playerInteractionManager.UpdateMesh();
        }

        [ClientRpc]
        private void RpcFakeDetachItem()
        {
            _playerManager.playerNetworkManager.myItem = null;
            _playerManager.playerInteractionManager.UpdateMesh();
        }

        [Command]
        public void CmdThrowItem()
        {
            Item item = _playerManager.playerNetworkManager.myItem.GetComponent<Item>();
            item.SetPosition(_playerManager.playerInteractionManager.positionToReleaseItems.position);
            item.SetRotation(Quaternion.identity);
            item.SetAvailable(true);
            item.StartThrownTimer();
            item.SetThrown(true);
            item.AddForce(10, transform.forward);
            RpcFakeDetachItem();
        }
        
    }
}
