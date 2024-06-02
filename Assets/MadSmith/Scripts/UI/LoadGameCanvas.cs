using System;
using System.Collections.Generic;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Systems.MenuController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MadSmith.Scripts.UI
{
    public class LoadGameCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject content;
        [SerializeField] private UISaveGameSlot uiSaveGameSlotPrefab;
        private readonly List<UISaveGameSlot> _saveGameSlots = new ();

        [SerializeField] private Page confirmDeletePopUp;
        [SerializeField] private Page hudPage;
        [SerializeField] private MenuController menuController;

        [Header("Save slot")] 
        private int _selectedSlot;
        private void Start()
        {
            if (!SaveGameManager.InstanceExists) return;

            for (var index = 0; index < SaveGameManager.Instance.gameSaveData.Length; index++)
            {
                UISaveGameSlot saveGameSlot = Instantiate(uiSaveGameSlotPrefab, content.transform);
                _saveGameSlots.Add(saveGameSlot);
                saveGameSlot.loadGameCanvas = this;
                saveGameSlot.gameDataSlotIndex = index;
                saveGameSlot.GetSaveSlotData();
            }
        }

        public void UpdateSlots()
        {
            foreach (var uiSaveGameSlot in _saveGameSlots)
            {
                uiSaveGameSlot.GetSaveSlotData();
            }
        }

        public void SelectSlot(int index)
        {
            _selectedSlot = index;
        }

        public void AttemptDeleteSaveSlot(InputAction.CallbackContext ctx)
        {
            Debug.Log(ctx.performed);
            if (ctx.performed)
            {
                Debug.Log("Delete:" + _selectedSlot);
                menuController.PushPage(confirmDeletePopUp);
            }
        }

        public void ConfirmDeleteSaveSlot()
        {
            if (!SaveGameManager.InstanceExists) return;
            SaveGameManager.Instance.DeleteGame(_selectedSlot);
            UpdateSlots();
        }

        public void LoadGame(int gameDataSlotIndex)
        {
            SaveGameManager.Instance.LoadGame(gameDataSlotIndex);
            menuController.PopAllPages();
            menuController.PushPage(hudPage);
        }
    }
}