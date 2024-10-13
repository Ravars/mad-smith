using System;
using System.Collections.Generic;
using System.Linq;
using MadSmith.Scripts.Managers;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class Table : Interactable
    {
        private readonly SyncList<Item> _items = new();
        public SlotHolder[] slotHolders;
        public CraftingTableType tableType;
        public override void OnStartClient()
        {
            _items.Callback += ItemsOnCallback;
        }

        private void ItemsOnCallback(SyncList<Item>.Operation op, int itemindex, Item olditem, Item newitem)
        {
            switch (op)
            {
                case SyncList<Item>.Operation.OP_ADD:
                    slotHolders[itemindex].SetMesh(newitem.baseItem.material, newitem.baseItem.mesh);
                    break;
                case SyncList<Item>.Operation.OP_CLEAR:
                    break;
                case SyncList<Item>.Operation.OP_INSERT:
                    break;
                case SyncList<Item>.Operation.OP_REMOVEAT:
                    slotHolders[itemindex].RemoveMesh();
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
            _items.Add(item);
        }

        [Server]
        public bool HasItem()
        {
            return _items.Count > 0;
        }

        [Server]
        public Item GetLastItem()
        {
            Item item = _items[^1];
            _items.Remove(item);
            return item;
        }

        [Server]
        public bool CanAddItem(Item newItem)
        {
            if (_items.Any(item => item.baseItem.itemName == newItem.baseItem.itemName)) return false;
            bool thereAreRecipesWithItems =  RecipesManager.Instance.ThereAreRecipesWithItems(new List<Item>(_items) { newItem }, tableType);
            return thereAreRecipesWithItems;
        }

        [Command(requiresAuthority = false)]
        public void CmdAttemptPickupThrownItem(GameObject itemGameObject)
        {
            if (itemGameObject == null) return;
            if (!itemGameObject.TryGetComponent(out Item item)) return;
            if (!CanAddItem(item)) return;
            
            AddItem(item);
        }
    }
}