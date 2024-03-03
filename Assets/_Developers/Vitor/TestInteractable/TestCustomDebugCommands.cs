using System;
using UnityEngine;
using IngameDebugConsole;
using Mirror;

namespace _Developers.Vitor.TestInteractable
{
    public class TestCustomDebugCommands : MonoBehaviour
    {
        [ConsoleMethod("host","Start hosting")]
        public static void StartHost()
        {
            NetworkManager.singleton.StartHost();
        }
        [ConsoleMethod("client","Start client to default ip address")]
        public static void StartClient()
        {
            NetworkManager.singleton.StartClient();
        } 
        [ConsoleMethod("client","Start client to ip address")]
        public static void StartClient(string ip)
        {
            if (ip != String.Empty)
            {
                NetworkManager.singleton.networkAddress = ip;
            }
            NetworkManager.singleton.StartClient();
        } 
    }
}