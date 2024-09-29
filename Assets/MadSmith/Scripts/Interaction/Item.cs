using System;
using _Developers.Vitor.Interactable;
using MadSmith.Scripts.BaseClasses;
using MadSmith.Scripts.Character.Player;
using UnityEngine;
using Mirror;


namespace MadSmith.Scripts.Interaction
{
    [DisallowMultipleComponent]
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

        [SyncVar] 
        private bool _thrown = false;

        [SerializeField] private float thrownDuration = 1.0f;
        private float _thrownTimer;
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

        // [Server]
        // public void SetRenderState(bool newState)
        // {
        //     itemMeshRender.enabled = newState;
        // }
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
                // else if(other.gameObject.CompareTag("Tables")){}
                else if (other.gameObject.CompareTag("Environment"))
                {
                    SetThrown(false);
                }
                
            }
        }
    }
}