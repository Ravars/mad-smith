using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Systems.MenuController;
using MadSmith.Scripts.Utils;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class HudPanel : Singleton<HudPanel>
    {
        [SerializeField] private MenuController menuController;
        [SerializeField] private Page settingsPage;
        [SerializeField] private Page hudPage;
        public BoolEventChannelSo changeGameStatus;

        private void OnEnable()
        {
            changeGameStatus.OnEventRaised += ChangeGameStatusOnEventRaised;
        }

        private void ChangeGameStatusOnEventRaised(bool arg0)
        {
            if (arg0)
            {
                PauseGame();
            }
            else
            {
                menuController.PopAllPages();
                menuController.PushPage(hudPage);
            }
        }

        public void PauseGame()
        {
            menuController.PushPage(settingsPage);
        }
    }
}