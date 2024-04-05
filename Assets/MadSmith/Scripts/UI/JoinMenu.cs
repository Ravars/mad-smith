using Mirror;
using TMPro;
using UnityEngine;
namespace MadSmith.Scripts.UI
{
    public class JoinMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        
        public void JoinGame(string ip)
        {
            NetworkManager.singleton.networkAddress = ip;
            NetworkManager.singleton.StartClient();
        }

        public void ButtonJoin()
        {
            JoinGame(inputField.text); 
        }
    }
}