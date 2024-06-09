using System;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class Interactable : MonoBehaviour
    {
        [HideInInspector] public bool showEditorButtons = true;

        [Header("Highlight properties")] 
        [SerializeField] private HighlightObject[] highlightObjects;
        private bool _currentState;

        public virtual void Awake()
        {
            SetStateHighlight(false);
        }


        public void SetStateHighlight(bool newState)
        {
            _currentState = newState;
            foreach (var highlightObject in highlightObjects)
            {
                highlightObject.meshRenderer.material = newState ? highlightObject.highlightMaterial: highlightObject.originalMaterial;
            }
        }
        public void SwitchMaterial()
        {
            _currentState = !_currentState;
            foreach (var highlightObject in highlightObjects)
            {
                highlightObject.meshRenderer.material = _currentState ? highlightObject.highlightMaterial: highlightObject.originalMaterial;
            }
        }
    }

    [Serializable]
    public struct HighlightObject
    {
        public Renderer meshRenderer;
        public Material originalMaterial;
        public Material highlightMaterial;
    }
}