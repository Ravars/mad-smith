﻿using System;
using System.Collections.Generic;
using _Developers.Vitor.Interactable;
using MadSmith.Scripts.BaseClasses;
using MadSmith.Scripts.Character.Player;
using MadSmith.Scripts.Managers;
using UnityEngine;
using Mirror;
using UnityEditor;


namespace MadSmith.Scripts.Interaction
{
    [DisallowMultipleComponent]
    public class Item : Interactable
    {
        private Rigidbody _rb;
        private float _thrownTimer;

        [Header("Item config")] 
        // [SyncVar(hook = nameof(OnBaseItemIdChange))]
        [SyncVar] public int baseItemId;
        
        public BaseItem baseItem;
        [SerializeField] private SphereCollider triggerOnGround;
        [SerializeField] public MeshFilter itemMeshFilter;
        [SerializeField] public MeshRenderer itemMeshRender;
        
        [SyncVar(hook = nameof(OnIsAvailableChange))]
        private bool _isAvailable = true;

        [SyncVar] private bool _thrown = false;

        [SerializeField] private float thrownDuration = 1.0f;
        public override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
        }

        public bool IsAvailable()
        {
            return _isAvailable;
        }

        public bool IsThrown()
        {
            return _thrown;
        }

        [Server]
        public void SetAvailable(bool state)
        {
            _isAvailable = state;
            _rb.isKinematic = !state;
            triggerOnGround.enabled = state;
        }

        [Server]
        public void SetThrown(bool state)
        {
            _thrown = state;
        }

        [Server]
        public void StartThrownTimer()
        {
            _thrownTimer = thrownDuration;
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

        [Server]
        public void AddForce(float impulseForce, Vector3 direction)
        {
            _rb.AddForce(impulseForce * direction, ForceMode.Impulse);
        }

        [ServerCallback]
        private void Update()
        {
            if (!_thrown) return;
            
            _thrownTimer -= Time.deltaTime;
            if (_thrownTimer <= 0f)
            {
                SetThrown(false);
            }
        }
        
        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (isServer)
            {
                if (_thrown && other.gameObject.CompareTag("Player"))
                {
                    other.GetComponent<PlayerNetworkManager>().CmdAttemptPickupThrownItem(gameObject);
                    SetThrown(false);
                }
                else if (_thrown && other.gameObject.CompareTag("Table"))
                {
                    Debug.Log("Table");
                    other.GetComponent<Table>().CmdAttemptPickupThrownItem(gameObject);
                    SetThrown(false);
                }
                else if (other.gameObject.CompareTag("Environment"))
                {
                    SetThrown(false);
                }
                
            }
        }
        
    }
}