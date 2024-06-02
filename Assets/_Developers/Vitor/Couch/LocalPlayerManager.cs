using System.Collections.Generic;
using MadSmith.Scripts.Character.Player;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.GameSaving;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = UnityEngine.InputSystem.PlayerInputManager;
using Random = UnityEngine.Random;

namespace _Developers.Vitor.Couch
{
    public class LocalPlayerManager : NetworkBehaviour
    {
        private GameInput _gameInput;
        private NetworkIdentity _potato;
        public int maxCouchPlayers = 4;
        public GameObject playerPrefab;
        // [SyncVar] public List<int> deviceIds = new List<int>();
        public readonly SyncList<int> deviceIds = new SyncList<int>();
        private PlayerInputManager _playerInputManager;

        private float timeLastSavedOrSpawned;
        private void Awake()
        {
            _playerInputManager = GetComponent<PlayerInputManager>();
            _playerInputManager.enabled = false;
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
            }

            timeLastSavedOrSpawned = Time.time;
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
            if (SaveGameManager.InstanceExists)
            {
                SaveGameManager.Instance.localPlayerManager = this;
            }
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
            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = Quaternion.identity;
            if (NetworkManager.startPositions.Count > 0)
            {
                Transform spawnObj = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)];
                spawnPosition = spawnObj.position;
                spawnRotation = spawnObj.rotation;
            }
            GameObject playerObj = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            PlayerManager playerManager = playerObj.GetComponent<PlayerManager>();
            NetworkServer.Spawn(playerObj, connectionToClient);
            playerManager.deviceId = deviceId;
        }

        public void SaveGameToCurrentGameData(ref GameSaveData gameSaveData)
        {
            gameSaveData.secondsPlayed += Time.time - timeLastSavedOrSpawned;
            timeLastSavedOrSpawned = Time.time;
        }

        public void LoadDataFromCurrentGameData(ref GameSaveData gameSaveData)
        {
            timeLastSavedOrSpawned = Time.time;
        }
    }
}