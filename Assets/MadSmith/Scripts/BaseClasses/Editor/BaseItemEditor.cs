using System;
using System.Collections.Generic;
using MadSmith.Scripts.Interaction;
using UnityEditor;
using UnityEngine;

namespace MadSmith.Scripts.BaseClasses.Editor
{
    [CustomEditor(typeof(Item))]
    public class BaseItemEditor : UnityEditor.Editor
    {
        private BaseItem selectedBaseItem;
        private List<BaseItem> allBaseItems;
        private int currentIndex = 0;
        private void OnEnable()
        {
            Debug.Log("On enable");
            allBaseItems = new List<BaseItem>(Resources.FindObjectsOfTypeAll<BaseItem>());

        }

        public override void OnInspectorGUI()
        {
            Item item = (Item)target;
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Editor Controls", EditorStyles.boldLabel);
            item.showEditorButtons = EditorGUILayout.Toggle("Show Dev tools", item.showEditorButtons);

            if (!item.showEditorButtons) return;
            EditorGUILayout.LabelField("Select Item Manually", EditorStyles.boldLabel);
            selectedBaseItem = (BaseItem)EditorGUILayout.ObjectField("Select BaseItem", selectedBaseItem, typeof(BaseItem), false);

            if (GUILayout.Button("Preencher do BaseItem"))
            {
                if (selectedBaseItem != null)
                {
                    AtualizarItem(item, selectedBaseItem);
                }
            }
            EditorGUILayout.LabelField("Rotate around all items", EditorStyles.boldLabel);

            if (allBaseItems.Count > 0)
            {
                // Exibe o nome do BaseItem atual
                EditorGUILayout.LabelField("BaseItem Atual:", allBaseItems[currentIndex].name);

                // Botão para o item anterior
                if (GUILayout.Button("Anterior"))
                {
                    currentIndex = (currentIndex - 1 + allBaseItems.Count) % allBaseItems.Count;
                    AtualizarItem(item,allBaseItems[currentIndex]);
                }

                // Botão para o próximo item
                if (GUILayout.Button("Próximo"))
                {
                    currentIndex = (currentIndex + 1) % allBaseItems.Count;
                    AtualizarItem(item,allBaseItems[currentIndex]);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Nenhum BaseItem encontrado no projeto.");
            }
        }

        private void AtualizarItem(Item item, BaseItem baseItem)
        {
            var itemBaseItem = baseItem;

            item.baseItem = itemBaseItem;
            item.itemMeshFilter.mesh = itemBaseItem.mesh;
            item.highlightObjects[0].originalMaterial = itemBaseItem.material;
            item.highlightObjects[0].highlightMaterial = itemBaseItem.materialHighlight;
            item.itemMeshRender.SetMaterials(new List<Material>(){itemBaseItem.material});
            EditorUtility.SetDirty(item);
        }
    }
}