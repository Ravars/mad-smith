using System.Collections.Generic;
using System.Linq;
using MadSmith.Scripts.BaseClasses;
using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;
using Utils;

namespace MadSmith.Scripts.Managers
{
    public class RecipesManager : PersistentSingleton<RecipesManager>
    {
        public Recipe[] recipes;
        
        [Server]
        public bool ThereAreRecipesWithItems(List<Item> inputItems, CraftingTableType craftingTableType)
        {
            return (
                    from recipe in recipes 
                    where recipe.craftingTableType == craftingTableType 
                    select inputItems.All(inputItem =>
                        recipe.requiredItems.Any(requiredItem => 
                            requiredItem.itemName == inputItem.baseItem.itemName))
                    ).Any(itemsMatch => itemsMatch);
        }
        
        [Server]
        public Recipe FindRecipe(List<Item> inputItems, CraftingTableType craftingTableType)
        {
            return (
                from recipe in recipes 
                where recipe.craftingTableType == craftingTableType 
                let exactMatch = recipe.requiredItems.Length == inputItems.Count 
                                 && recipe.requiredItems.All(requiredItem => 
                                     inputItems.Any(inputItem => 
                                         inputItem.baseItem.itemName == requiredItem.itemName)) 
                where exactMatch select recipe).FirstOrDefault();
        }
    }
}