using System;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.EditorHelpers
{
    public class AutoStartHosting : MonoBehaviour
    {
        [SerializeField] private bool hostOnStart = true;

        private void Start()
        {
            if (hostOnStart)
            {
                NetworkManager.singleton.StartHost();
            }
        }
    }
}