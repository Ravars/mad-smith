using System;
using Mirror;
using TMPro;
using MadSmith.Scripts.Character.Player;
using UnityEngine;

namespace _Developers.Vitor.Interactable
{
    public class TestItem : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnMyNumberChanged))] public int myNumber;
        [SyncVar] public int myNumber2;
        [SyncVar] public int myNumber3;
        [SyncVar] public int myNumber4;
        [SyncVar] public int myNumber5;
        public PlayerInputManager playerInputManager;
        public PlayerManager _playerManager;
        public TextMeshProUGUI myNumberText;

        private void Start()
        {
            playerInputManager.InputReader.AttackEvent += InputReaderOnAttackEvent;
        }

        private void InputReaderOnAttackEvent(int deviceId)
        {
            if (deviceId != _playerManager.deviceId) return;
            if (!authority) return;
            myNumber5 += 5;
            CmdChangeNumber(myNumber5);
        }

        [Command]
        public void CmdChangeNumber(int newNumber5)
        {
            myNumber += 1;
            myNumber2 += 2;
            RpcUpdate(myNumber3 + 3);
            // newNumber5 = newNumber5;
            if (isServer)
            {
                myNumber4 += 4;
            }
            // myNumber3 += 3;
        }
        public void OnMyNumberChanged(int oldNumber, int newNumber)
        {
            Debug.Log($"Number changed from {oldNumber} to {newNumber}");
            UpdateText(newNumber);
        }
        public void UpdateText(int newNumber)
        {
            myNumberText.text = "Number: "+newNumber;
        }

        [ClientRpc]
        public void RpcUpdate(int newValue)
        {
            myNumber3 = newValue;
        }

    }
}