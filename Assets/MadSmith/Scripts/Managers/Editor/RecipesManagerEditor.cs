using MadSmith.Scripts.BaseClasses;
using MadSmith.Scripts.Interaction;
using UnityEditor;
using UnityEngine;

namespace MadSmith.Scripts.Managers.Editor
{
    [CustomEditor(typeof(RecipesManager))]
    public class RecipesManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            RecipesManager recipesManager = (RecipesManager)target;

            if (GUILayout.Button("Get all recipes"))
            {
                GetAllRecipes(recipesManager);
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Get all items"))
            {
                GetAllItems(recipesManager);
            }
            if (GUILayout.Button("Update items IDs"))
            {
                UpdateIds(recipesManager);
            }
            GUILayout.EndHorizontal();
        }

        private void GetAllRecipes(RecipesManager recipesManager)
        {
            Recipe[] allRecipe = Resources.FindObjectsOfTypeAll<Recipe>();
            recipesManager.recipes = allRecipe;
            EditorUtility.SetDirty(recipesManager);
        }
        private void GetAllItems(RecipesManager recipesManager)
        {
            BaseItem[] allRecipe = Resources.FindObjectsOfTypeAll<BaseItem>();
            recipesManager.baseItems = allRecipe;
            EditorUtility.SetDirty(recipesManager);
        }

        private void UpdateIds(RecipesManager recipesManager)
        {
            for (int i = 0; i < recipesManager.baseItems.Length; i++)
            {
                recipesManager.baseItems[i].id = i+1;
            }
        }
    }
}