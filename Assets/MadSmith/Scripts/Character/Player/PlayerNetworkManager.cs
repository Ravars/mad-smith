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
        public void CmdPickupItem(GameObject itemGameObject)
        {
            Item item = itemGameObject.GetComponent<Item>();

            if (_playerManager.playerNetworkManager.myItem != null)
            {
                Debug.Log("already has item!");
                return;
                //TODO: testar
            }
            
            
            if (item != null && item.IsAvailable())
            {
                item.SetAvailable(false);
                // _playerManager.playerNetworkManager.myItem = itemGameObject;
                RpcFakeAttachItem(itemGameObject);
            }
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
            // item.set
        }

        [ClientRpc]
        private void RpcFakeDetachItem()
        {
            _playerManager.playerNetworkManager.myItem = null;
            UpdateMesh();
        }
        private void UpdateMesh()
        {
            Debug.Log("Update mesh: " + myItem);
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
        

        // [Command]
        // public void CmdAttemptGrabItemFakeMesh(GameObject gameObjectItem)
        // {
        //     if (ReferenceEquals(gameObjectItem, null)) return;
        //     if (!gameObjectItem.TryGetComponent(out Item item)) return;
        //     if (!item.IsAvailable()) return;
        //
        //     _playerManager.playerInventoryManager.item = item;
        //     
        //     item.SetAvailable(false);
        //     myItem = gameObjectItem;
        //     // handMeshRenderer.material = item.baseItem.material;
        //     // handMeshFilter.mesh = item.baseItem.mesh;
        //     RpcSetMesh(gameObjectItem);
        // }
        //
        // [ClientRpc]
        // public void RpcSetMesh(GameObject gameObjectItem)
        // {
        //     if (!gameObjectItem.TryGetComponent(out Item item)) return;
        //     handMeshRenderer.material = item.baseItem.material;
        //     handMeshFilter.mesh = item.baseItem.mesh;
        //     if (isServer)
        //     { 
        //         item.SetState(false);
        //     }
        // }
        //
        // [Command]
        // public void CmdDropItem(GameObject itemToDrop, Vector3 pointToSpawn)
        // {
        //     if (myItem == null) return;
        //     _playerManager.playerInventoryManager.item = null;
        //     RpcDrop(itemToDrop, pointToSpawn);
        // }
        //
        // [ClientRpc]
        // private void RpcDrop(GameObject itemToDrop, Vector3 pointToSpawn)
        // {
        //     Debug.Log("MyItem:" + myItem.name);
        //     Debug.Log("itemToDrop:" + itemToDrop.name);
        //     if (!itemToDrop.TryGetComponent(out Item item)) return;
        //     item.transform.position = pointToSpawn;
        //     if (isServer)
        //     { 
        //         // item.SetAvailable(true);
        //         item.SetState(true);
        //     }
        //     myItem = null;
        // }
        
        
        
        // #region Funcionou mas segundo a documentação ta errado, Talvez mudar para um soft parent
        //
        //
        //
        // [Command]
        // public void AttemptGrabItem(GameObject itemGameObject, GameObject parentToAttach)
        // {
        //     if (ReferenceEquals(itemGameObject, null)) return;
        //     if (ReferenceEquals(parentToAttach, null)) return;
        //     if (!itemGameObject.TryGetComponent(out Item item)) return;
        //     if (!parentToAttach.TryGetComponent(out NetworkIdentity networkIdentityParentToAttach)) return;
        //     if (!item.IsAvailable()) return;
        //     
        //     item.SetAvailable(false);
        //     RpcAttachItemToPlayer(itemGameObject,parentToAttach, netId);
        //     
        //     
        //     // item.transform.parent = rightHand;
        //     // item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //     // item.SetStateCollider(false);
        //     // item.SetStatePhysics(false);
        //     // _playerManager.playerInventoryManager.item = item;
        // }
        //
        // [ClientRpc]
        // private void RpcAttachItemToPlayer(GameObject itemGameObject, GameObject parentToAttach, uint playerNetId)
        // {
        //     // NetworkIdentity.
        //     // GameObject player = NetworkIdentity.GetSceneIdentity(playerNetId).gameObject;
        //     Debug.Log("Player:" + parentToAttach.name);
        //
        //     if (parentToAttach != null)
        //     {
        //         itemGameObject.transform.SetParent(parentToAttach.transform);
        //         itemGameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //         // item.SetStateCollider(false);
        //         // item.SetStatePhysics(false);
        //     }
        // }
        // #endregion
    }
}
