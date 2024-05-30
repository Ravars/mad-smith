using System;
using MadSmith.Scripts.GameSaving;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class UISaveGameSlot : MonoBehaviour
    {
        private SaveFileDataWriter _saveFileDataWriter;

        [Header("Game Slot")] public GameDataSlot gameDataSlot;

        [Header("Info")]
        [SerializeField] private TextMeshProUGUI timePlayedText;
        [SerializeField] private TextMeshProUGUI nameText;
        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            if (!TemporarySaveGameManager.InstanceExists)
            {
                Debug.LogError("SaveGameManager not found");
                return;
            }
            
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            _saveFileDataWriter.saveFileName =
                TemporarySaveGameManager.Instance.DecideGameDataFileNameBasedOnSlotBeingUsed(gameDataSlot);
            GameSaveData gameSaveData = TemporarySaveGameManager.Instance.GetGameDataSlotByName(gameDataSlot);
            if (gameSaveData is { loaded: true })
            {
                nameText.text = gameDataSlot.ToString();
                timePlayedText.text = TimeFormatter.FormatTime((int)gameSaveData.secondsPlayed);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
    
}