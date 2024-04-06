using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Network.Managers
{
    public class MadSmithNetworkManager : NetworkManager
    {
        public override void Start()
        {
            base.Start();
            transport.OnClientDisconnected += OnClientDisconnected;
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