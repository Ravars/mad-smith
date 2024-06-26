using System;
using MadSmith.Scripts.BaseClasses;
using UnityEngine;
using Mirror;

namespace MadSmith.Scripts.Interaction
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Item : Interactable
    {
        [Header("Item config")]
        public BaseItem baseItem;
        [SerializeField] private SphereCollider triggerOnGround;
        private Rigidbody _rb;

        [SerializeField] private MeshFilter itemMeshFilter;
        [SerializeField] private MeshRenderer itemMeshRender;
        
        [SyncVar(hook = nameof(OnIsAvailableChange))]
        private bool _isAvailable = true;

        public override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
        }

        public bool IsAvailable()
        {
            return _isAvailable;
        }

        [Server]
        public void SetAvailable(bool state)
        {
            _isAvailable = state;
            _rb.isKinematic = !state;
            triggerOnGround.enabled = state;
        }

        [Server]
        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        [Server]
        public void SetRotation(Quaternion newRotation)
        {
            transform.rotation = newRotation;
        }
        private void OnIsAvailableChange(bool oldState, bool newState)
        {
            itemMeshRender.enabled = newState;
        }
    }
}