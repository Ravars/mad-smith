using System.Collections.Generic;
using System.Linq;
using MadSmith.Scripts.Player;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace _Developers.Vitor.TestCoop
{
    public class CoopManager : NetworkBehaviour
    {
        private List<PlayerManager> _playerList = new List<PlayerManager>();
        [SerializeField] private GameObject playerPrefab;
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            if (isLocalPlayer)
            {
                CmdCallAddPlayer(0);
            }
        }
        
        public void CmdCallAddPlayer(int playerNumber, int deviceId = -1)
        {
            if (deviceId == -1)
            {
                CmdAddPlayer(playerNumber,deviceId);
            }
            else
            {
                var newPlayer = IsNewDevice(deviceId);
                if (newPlayer)
                {
                    CmdAddPlayer(playerNumber, deviceId);
                }
                
            }
        }

        public bool IsNewDevice(int deviceId)
        {
            return _playerList.All(x => x.InputReader.deviceId != deviceId);
        }
        
        [Command]
        public void CmdAddPlayer(int playerNumber, int deviceId)
        {
            // Debug.Log("CmdAddPlayer " + deviceId + " " + playerNumber);
            Transform spawnObj = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)];
            GameObject playerObj = Instantiate(playerPrefab, spawnObj.position, spawnObj.rotation);
            PlayerManager couchPlayer = playerObj.GetComponent<PlayerManager>();
            couchPlayer.playerNumber = _playerList.Count;
            _playerList.Add(couchPlayer);
            couchPlayer.playerNumber = playerNumber;
            couchPlayer.deviceId = deviceId;
            couchPlayer.coopManager = this;
            NetworkServer.Spawn(playerObj, connectionToClient);
            // couchPlayer.InputReader.SetDeviceId(playerNumber == 0 ? 1 : 18);
            //TODO: Lidar ainda com o Control Scheme
        }
    }
}