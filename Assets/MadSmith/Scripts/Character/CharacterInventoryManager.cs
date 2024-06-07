using MadSmith.Scripts.Interaction;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        public Item item;
        //TODO: Fazer o controle das armas

        public bool IsHoldingItem()
        {
            return item != null;
        }
    }
}