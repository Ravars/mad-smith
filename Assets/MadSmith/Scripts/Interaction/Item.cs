using System;
using MadSmith.Scripts.BaseClasses;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    [DisallowMultipleComponent]
    public class Item : Interactable
    {
        [Header("Item config")]
        public BaseItem baseItem;
        [SerializeField] private SphereCollider triggerOnGround;
        private Rigidbody _rb;

        public override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
        }

        public void SetStateCollider(bool newState)
        {
            triggerOnGround.enabled = newState;
        }

        public void SetStatePhysics(bool newState)
        {
            _rb.isKinematic = !newState;
        }
    }
}