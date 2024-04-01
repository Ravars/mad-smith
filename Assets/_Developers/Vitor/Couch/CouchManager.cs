using System.Collections.Generic;
using MadSmith.Scripts.Character.Player;
using MadSmith.Scripts.Input;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = UnityEngine.InputSystem.PlayerInputManager;
using Random = UnityEngine.Random;

namespace _Developers.Vitor.Couch
{
    public class CouchManager : NetworkBehaviour
    {
        private GameInput _gameInput;
        private NetworkIdentity _potato;
        public int maxCouchPlayers = 4;
        public GameObject playerPrefab;
        // [SyncVar] public List<int> deviceIds = new List<int>();
        public readonly SyncList<int> deviceIds = new SyncList<int>();
        private PlayerInputManager _playerInputManager;
        private void Awake()
        {
            _playerInputManager = GetComponent<PlayerInputManager>();
            _playerInputManager.enabled = false;
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
            }
            // _playerInputManager.onPlayerJoined += PlayerInputManagerOnPlayerJoined;
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            Debug.Log("isOwned: " + isOwned);
            _playerInputManager.enabled = true;
            _gameInput.CouchJoin.Enable();
            _gameInput.CouchJoin.Join.performed += JoinOnPerformed;
            _playerInputManager.onPlayerLeft += PlayerInputManagerOnPlayerLeft;
        }

        private void PlayerInputManagerOnPlayerLeft(PlayerInput obj)
        {
            Debug.Log("Player left");
            // throw new System.NotImplementedException();
        }

        private void JoinOnPerformed(InputAction.CallbackContext obj)
        {
            if (!authority || !isOwned) return;
            if (!deviceIds.Contains(obj.control.device.deviceId) && deviceIds.Count < maxCouchPlayers)
            {
                deviceIds.Add(obj.control.device.deviceId);
                CmdAddPlayer(obj.control.device.deviceId);
            }
        }

        [Command]
        private void CmdAddPlayer(int deviceId)
        {
            Transform spawnObj = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)];
            GameObject playerObj = Instantiate(playerPrefab, spawnObj.position, spawnObj.rotation);
            PlayerManager playerManager = playerObj.GetComponent<PlayerManager>();
            NetworkServer.Spawn(playerObj, connectionToClient);
            playerManager.deviceId = deviceId;
        }
    }
}