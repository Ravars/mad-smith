using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Systems.MenuController;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class LobbyMainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private Page hudPage;
        [SerializeField] private MenuController menuController;
        
        public void btn_NewGame()
        {
            if (!SaveGameManager.InstanceExists) return;
            
            bool newGame = SaveGameManager.Instance.AttemptCreateNewGame();

            if (newGame)
            {
                menuController.PopAllPages();
                menuController.PushPage(hudPage);
            }
            else
            {
                if (NotificationManager.InstanceExists)
                {
                    NotificationManager.Instance.NewNotification("Não há slots disponíveis.", NotificationType.Warning);
                }
            }
        }
    }
}