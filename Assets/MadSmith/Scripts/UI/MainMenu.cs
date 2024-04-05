using IngameDebugConsole;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [ConsoleMethod("quit", "Close the game")]
        public static void ButtonQuitGame()
        {
            Application.Quit();
        }

        public void ButtonHostGame()
        {
            NetworkManager.singleton.StartHost();
        }
    }
}