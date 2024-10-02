using System;
using UnityEngine;

namespace MadSmith.Scripts.BaseClasses
{
    [CreateAssetMenu(fileName = "newItem", menuName = "Game/Item", order = 0)]
    public class BaseItem : ScriptableObject
    {
        public string itemName;
        public Sprite itemImage;
        public Mesh mesh;
        public Material material;
        public Material materialHighlight;
        public PositionRotationScaleItem[] positionRotationScaleItemForEachTable;

    }
    [Serializable]
    public struct PositionRotationScaleItem
    {
        public CraftingTableType craftingTableType;
        public Vector3 scale;
        public Vector3 offSet;
        public Quaternion rotation;
    }
    
}