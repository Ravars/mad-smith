using System;
using MadSmith.Scripts.BaseClasses;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class Item : MonoBehaviour
    {
        public BaseItem baseItem;
        [SerializeField] private SphereCollider triggerOnGround;
        [SerializeField] private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void SetStateCollider(bool newState)
        {
            triggerOnGround.enabled = newState;
        }

        public void SetStatePhysics(bool newState)
        {
            rb.isKinematic = !newState;
        }
    }
}