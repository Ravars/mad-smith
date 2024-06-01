using Mirror;
using UnityEngine;

#if UNITY_EDITOR
using ParrelSync;
#endif


namespace MadSmith.Scripts.Network.Managers
{
    public class MadSmithNetworkManager : NetworkManager
    {

        [Header("Auto Host options")] 
        [SerializeField] private bool autoHost = true;

        [Header("Auto join options")] 
        [SerializeField] private bool autoJoin = false;
        [SerializeField] private string ip;
        
        public override void Start()
        {
            base.Start();
            transport.OnClientDisconnected += OnClientDisconnected;
            #if UNITY_EDITOR
            // TODO: test this. Maybe use some custom tag to enable auto play
            if (ClonesManager.IsClone())
            {
                autoJoin = true;
            }
            if (autoHost)
            {
                StartHost();
            }else if (autoJoin)
            {
                networkAddress = ip;
                StartClient();
            }
            #endif
        }

        private void OnClientDisconnected()
        {
            Debug.Log("OnClientDisconnected");
        }

        public override void OnClientError(TransportError error, string reason)
        {
            base.OnClientError(error, reason);
            Debug.Log(reason);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("Client Start");
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            Debug.Log("OnClientDisconnect");
        }
        
    }
}