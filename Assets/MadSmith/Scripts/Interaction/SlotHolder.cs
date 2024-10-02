using System;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class SlotHolder : MonoBehaviour
    {
        public ItemSlotHolderCategory itemSlotHolderCategory;
        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
        }

        public void SetMesh(Material newMaterial, Mesh newMesh)
        {
            meshFilter.mesh = newMesh;
            meshRenderer.material = newMaterial;
        }

        public void RemoveMesh()
        {
            meshFilter.mesh = null;
            meshRenderer.material = null;
        }
    }
}