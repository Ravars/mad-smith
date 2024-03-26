using IngameDebugConsole;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [ConsoleMethod("quit", "Close the game")]
        public void ButtonQuitGame()
        {
            Application.Quit();
        }
    }
}