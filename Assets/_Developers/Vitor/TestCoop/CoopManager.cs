using System.Collections.Generic;
using MadSmith.Scripts.Player;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Developers.Vitor.TestCoop
{
    public class CoopManager : NetworkBehaviour
    {
        private List<GameObject> _playerList = new List<GameObject>();
        [SerializeField] private GameObject playerPrefab;
        [ContextMenu("Add Player 2")]
        public void CallAddPlayer()
        {
            if (isServer)
            {
                CmdAddPlayer();
            }
        }
        
        [Command]
        public void CmdAddPlayer()
        {
            Transform spawnObj = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)];
            GameObject playerObj = Instantiate(playerPrefab, spawnObj.position, spawnObj.rotation);
            PlayerManager couchPlayer = playerObj.GetComponent<PlayerManager>();
            couchPlayer.playerNumber = _playerList.Count;
            NetworkServer.Spawn(playerObj, connectionToClient);
            _playerList.Add(playerObj);
            //TODO: Lidar ainda com o Control Scheme
        }
    }
}