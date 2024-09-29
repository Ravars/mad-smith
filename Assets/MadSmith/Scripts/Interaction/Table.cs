using System;
using System.Linq;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class Table : Interactable
    {
        public readonly SyncList<Item> items = new SyncList<Item>();
        public Transform positionToItems;

        public override void OnStartClient()
        {
            items.Callback += ItemsOnCallback;
        }

        private void ItemsOnCallback(SyncList<Item>.Operation op, int itemindex, Item olditem, Item newitem)
        {
            switch (op)
            {
                case SyncList<Item>.Operation.OP_ADD:
                    Debug.Log("Item added");
                    break;
                case SyncList<Item>.Operation.OP_CLEAR:
                    break;
                case SyncList<Item>.Operation.OP_INSERT:
                    break;
                case SyncList<Item>.Operation.OP_REMOVEAT:
                    break;
                case SyncList<Item>.Operation.OP_SET:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }

        [Server]
        public void AddItem(Item item)
        {
            item.SetAvailable(false);
            items.Add(item);
        }

        [Server]
        public bool HasItem()
        {
            return items.Any(x => x.IsAvailable());
            // return items.Count > 0;
        }

        [Server]
        public Item GetLastItem()
        {
            Item item = items[^1];
            items.Remove(item);
            return item;
        }

        [Server]
        public bool CanAddItem(Item newItem)
        {
            return true;
        }

        [Command(requiresAuthority = false)]
        public void CmdAttemptPickupThrownItem(GameObject itemGameObject)
        {
            Debug.Log("CmdAttemptPickupThrownItem");
            if (itemGameObject == null) return;
            Debug.Log("null");
            if (!itemGameObject.TryGetComponent(out Item item)) return;
            Debug.Log("not items");
            if (!CanAddItem(item)) return;
            Debug.Log("can add");
            
            AddItem(item);
            item.SetPosition(positionToItems.position);
            item.SetRotation(Quaternion.identity);
            item.SetAvailable(true);
        }
        
        //TODO: Maybe add a "playerInteractionManager.UpdateMesh"-like 
    }
}