using UnityEditor;
using UnityEngine;

namespace MadSmith.Scripts.Interaction.Editor
{
    [CustomEditor(typeof(Interactable), true)]
    public class InteractableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Interactable materialSwitcher = (Interactable)target;
            
            DrawDefaultInspector();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Editor Controls", EditorStyles.boldLabel);
            materialSwitcher.showEditorButtons = EditorGUILayout.Toggle("Show Dev tools", materialSwitcher.showEditorButtons);

            if (!materialSwitcher.showEditorButtons) return;
            if (GUILayout.Button("Switch Material"))
            {
                materialSwitcher.SwitchMaterial();
            }
        }
    }
}