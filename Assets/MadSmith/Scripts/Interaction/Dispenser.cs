using MadSmith.Scripts.BaseClasses;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class Dispenser : Interactable
    {
        public Item itemPrefab;

        [Server]
        public Item SpawnItem()
        {
            Item itemSpawned = Instantiate(itemPrefab);
            NetworkServer.Spawn(itemSpawned.gameObject);
            return itemSpawned;
        }
    }
}