using System;
using IngameDebugConsole;
using Mirror;
using UnityEngine;

namespace MadSmith.DebugTools
{
    public class NetworkCustomDebugCommands : MonoBehaviour
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