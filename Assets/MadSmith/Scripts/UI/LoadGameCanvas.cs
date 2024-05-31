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
        [SerializeField] private MenuController menuController;

        [Header("Save slot")] 
        private int _selectedSlot;
        private void Start()
        {
            if (!TemporarySaveGameManager.InstanceExists) return;

            for (var index = 0; index < TemporarySaveGameManager.Instance.gameSaveData.Length; index++)
            {
                var gameSaveData = TemporarySaveGameManager.Instance.gameSaveData[index];
                UISaveGameSlot saveGameSlot = Instantiate(uiSaveGameSlotPrefab, content.transform);
                _saveGameSlots.Add(saveGameSlot);
                saveGameSlot.loadGameCanvas = this;
                saveGameSlot.gameDataSlotIndex = index;
                saveGameSlot.LoadSaveSlot();
            }
        }

        public void UpdateSlots()
        {
            foreach (var uiSaveGameSlot in _saveGameSlots)
            {
                uiSaveGameSlot.LoadSaveSlot();
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
            if (!TemporarySaveGameManager.InstanceExists) return;
            TemporarySaveGameManager.Instance.DeleteGame(_selectedSlot);
            UpdateSlots();
        }
    }
}