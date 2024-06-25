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
        
        private void UpdateMesh()
        {
            if (itemMeshFilter != null && itemMeshRender != null && baseItem != null)
            {
                itemMeshFilter.mesh = baseItem.mesh;
                itemMeshRender.material = baseItem.material;
            }
        }
        //
        // [SyncVar(hook = nameof(OnStateChange))] private bool _state = true;
        //
        //
        // public override void Awake()
        // {
        //     base.Awake();
        //     _rb = GetComponent<Rigidbody>();
        //     itemMeshFilter = GetComponentInChildren<MeshFilter>();
        //     itemMeshRender = GetComponentInChildren<MeshRenderer>();
        // }
        //
        // public void SetStateCollider(bool newState)
        // {
        //     triggerOnGround.enabled = newState;
        // }
        //
        // public void SetStatePhysics(bool newState)
        // {
        //     _rb.isKinematic = !newState;
        // }
        //
        //
        // [Server]
        // public void SetAvailable(bool state)
        // {
        //     _isAvailable = state;
        //     SetStatePhysics(state);
        //     SetStateCollider(state);
        // }
        //
        //
        // [Server]
        // public void SetState(bool state)
        // {
        //     _state = state;
        // }
        //
        private void OnIsAvailableChange(bool oldState, bool newState)
        {
            itemMeshRender.enabled = newState;
        }
        // private void OnStateChange(bool oldState, bool newState)
        // {
        //     itemMeshRender.enabled = newState;
        //     _rb.isKinematic = !newState;
        //     // itemMeshFilter.
        // }
    }
}