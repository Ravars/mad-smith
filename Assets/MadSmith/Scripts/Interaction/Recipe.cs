using MadSmith.Scripts.BaseClasses;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    [CreateAssetMenu(fileName = "newRecipe", menuName = "Game/Recipe")]
    public class Recipe : DescriptionBaseSo
    {
        public BaseItem[] requiredItems;
        public BaseItem generatedItem;
        public CraftingTableType craftingTableType;
    }
}