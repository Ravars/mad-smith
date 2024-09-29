using MadSmith.Scripts.Interaction;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        private PlayerManager _playerManager;
        
        protected override void Awake()
        {
            base.Awake();
            _playerManager = GetComponent<PlayerManager>();
        }

        public bool IsHoldingItem()
        {
            return _playerManager.playerNetworkManager.myItem != null;
        }

        public Item GetItem()
        {
            return _playerManager.playerNetworkManager.myItem.GetComponent<Item>();
        }
        
    }
}