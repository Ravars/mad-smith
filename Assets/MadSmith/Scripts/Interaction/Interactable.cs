using System;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class Interactable : NetworkBehaviour
    {
        [HideInInspector] public bool showEditorButtons = true;

        [Header("Highlight properties")] 
        [SerializeField] private HighlightObject[] highlightObjects;
        private bool _currentHighlightState;

        public virtual void Awake()
        {
            SetStateHighlight(false);
        }
        public virtual void SetStateHighlight(bool newState)
        {
            _currentHighlightState = newState;
            foreach (var highlightObject in highlightObjects)
            {
                highlightObject.meshRenderer.material = newState ? highlightObject.highlightMaterial: highlightObject.originalMaterial;
            }
        }
        public virtual void SwitchMaterial()
        {
            _currentHighlightState = !_currentHighlightState;
            foreach (var highlightObject in highlightObjects)
            {
                highlightObject.meshRenderer.material = _currentHighlightState ? highlightObject.highlightMaterial: highlightObject.originalMaterial;
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