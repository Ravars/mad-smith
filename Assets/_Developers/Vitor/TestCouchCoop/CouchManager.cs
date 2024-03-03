using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace _Developers.Vitor.TestCouchCoop
{
    public class CouchManager : Singleton<CouchManager>
    {
        private PlayerInputManager _playerInputManager;

        protected override void Awake()
        {
            base.Awake();
            _playerInputManager = GetComponent<PlayerInputManager>();
        }

        private void OnEnable()
        {
            Debug.Log("Enable");
            _playerInputManager.onPlayerJoined += PlayerInputManagerOnPlayerJoined;
        }

        private void OnDisable()
        {
            if (_playerInputManager != null)
            {
                Debug.Log("Disable");
                _playerInputManager.onPlayerJoined -= PlayerInputManagerOnPlayerJoined;
            }
        }

        private void PlayerInputManagerOnPlayerJoined(PlayerInput obj)
        {
            Debug.Log("Player Joined");
            foreach (var inputDevice in obj.devices)
            {
                Debug.Log(inputDevice.name);
                Debug.Log(inputDevice.device);
                Debug.Log(inputDevice.deviceId);
            }
        }
    }
}
