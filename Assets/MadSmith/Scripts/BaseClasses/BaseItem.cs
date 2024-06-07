using System;
using UnityEngine;

namespace MadSmith.Scripts.BaseClasses
{
    [CreateAssetMenu(fileName = "newItem", menuName = "Game/Item", order = 0)]
    public class BaseItem : ScriptableObject
    {
        public string itemName;
        public Sprite itemImage;
    }
}