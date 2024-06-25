using System;
using MadSmith.Scripts.Interaction;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        private PlayerManager _playerManager;
        
        [SyncVar] public GameObject myItem;
        
        public MeshRenderer handMeshRenderer;
        public MeshFilter handMeshFilter;

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        public void Init()
        {
            UpdateMesh();
        }
        [Command]
        public void CmdAttemptPickupItem(GameObject itemGameObject)
        {
            if (itemGameObject == null) return;
            if (!itemGameObject.TryGetComponent(out Item item) || !item.IsAvailable()) return;
            
            item.SetAvailable(false);
            RpcFakeAttachItem(itemGameObject);
        }

        [Command]
        public void CmdReleaseItem()
        {
            Item item = _playerManager.playerNetworkManager.myItem.GetComponent<Item>();
            item.SetPosition(_playerManager.playerInteractionManager.positionToReleaseItems.position);
            item.SetAvailable(true);
            RpcFakeDetachItem();
        }

        [ClientRpc]
        private void RpcFakeAttachItem(GameObject itemGameObject)
        {   
            _playerManager.playerNetworkManager.myItem = itemGameObject;
            UpdateMesh();
        }

        [ClientRpc]
        private void RpcFakeDetachItem()
        {
            _playerManager.playerNetworkManager.myItem = null;
            UpdateMesh();
        }
        private void UpdateMesh()
        {
            if (myItem != null && myItem.TryGetComponent(out Item item))
            {
                handMeshRenderer.material = item.baseItem.material;
                handMeshFilter.mesh = item.baseItem.mesh;
            }
            else
            {
                handMeshRenderer.material = null;
                handMeshFilter.mesh = null;
            }
        }
    }
}
