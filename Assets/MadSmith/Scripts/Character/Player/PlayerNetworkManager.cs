﻿using System;
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

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }
        

        #region Mirror Events
        
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            Debug.Log("Start authority manager");
            // enabled = true;
            _playerManager.playerLocomotionManager.enabled = true;
            _playerManager.characterController.enabled = true; 
            _playerManager.playerInputManager.enabled = true;
            _playerManager.playerInteractionManager.enabled = true;
            _playerManager.playerInputManager.InputReader.EnableGameplayInput();
            _playerManager.settingsPanelState.OnEventRaised += _playerManager.SettingsPanelStateOnEventRaised;
        }
        #endregion

        [Command]
        public void CmdAttemptPickupItem(GameObject itemGameObject)
        {
            if (itemGameObject == null) return; //TODO: tentar mudar para referenceEquals
            if (!itemGameObject.TryGetComponent(out Item item) || !item.IsAvailable()) return;
            
            item.SetAvailable(false);
            RpcFakeAttachItem(itemGameObject);
        }

        [Command]
        public void CmdAttemptPlaceItem(Item item, Table tableRef)
        {
            Debug.Log("Attempt place cmd");
            if (!tableRef.CanAddItem(item)) return;
            
            tableRef.AddItem(item);
            // item.SetPosition(tableRef.positionToItems[0].position);
            // item.SetRotation(Quaternion.identity);
            // item.SetAvailable(true);
            RpcFakeDetachItem();
        }

        [Command]
        public void CmdAttemptPickupTableItem(Table tableRef)
        {
            if (tableRef == null) return;
            if (!tableRef.HasItem()) return;
            Item item = tableRef.GetLastItem();
            item.SetAvailable(false);
            RpcFakeAttachItem(item.gameObject);
        }

        [Command]
        public void CmdAttemptPickupDispenserItem(Dispenser dispenserRef)
        {
            if (dispenserRef == null) return;
            Item item = dispenserRef.SpawnItem();
            item.SetAvailable(false);
            RpcFakeAttachItem(item.gameObject);
        }

        [Command(requiresAuthority = false)]
        public void CmdAttemptPickupThrownItem(GameObject itemGameObject)
        {
            if (_playerManager.isDead) return;
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