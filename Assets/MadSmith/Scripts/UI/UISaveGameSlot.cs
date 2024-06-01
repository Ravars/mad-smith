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

        public LoadGameCanvas loadGameCanvas;
        private SaveFileDataWriter _saveFileDataWriter;

        [Header("Game Slot")] 
        public int gameDataSlotIndex;

        [Header("Info")]
        [SerializeField] private TextMeshProUGUI timePlayedText;
        [SerializeField] private TextMeshProUGUI nameText;

        public void GetSaveSlotData()
        {
            if (!TemporarySaveGameManager.InstanceExists)
            {
                Debug.LogError("SaveGameManager not found");
                return;
            }
            
            _saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = TemporarySaveGameManager.DecideGameDataFileNameBasedOnIndex(gameDataSlotIndex)
            };
            GameSaveData gameSaveData = TemporarySaveGameManager.Instance.GetGameDataSlotByIndex(gameDataSlotIndex);
            if (gameSaveData is { loaded: true })
            {
                nameText.text = gameDataSlotIndex.ToString();
                timePlayedText.text = TimeFormatter.FormatTime((int)gameSaveData.secondsPlayed);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void LoadGameFromGameSlot()
        {
            loadGameCanvas.LoadGame(gameDataSlotIndex);
        }

        public void SetSelectedSlot()
        {
            if (ReferenceEquals(loadGameCanvas, null)) return;
            loadGameCanvas.SelectSlot(gameDataSlotIndex);
        }
    }
    
}