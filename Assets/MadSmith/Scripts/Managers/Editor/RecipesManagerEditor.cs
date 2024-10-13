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
        }

        private void GetAllRecipes(RecipesManager recipesManager)
        {
            Recipe[] allRecipe = Resources.FindObjectsOfTypeAll<Recipe>();
            recipesManager.recipes = allRecipe;
            EditorUtility.SetDirty(recipesManager);
        }
    }
}