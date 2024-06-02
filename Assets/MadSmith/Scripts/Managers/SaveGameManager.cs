using System;
using _Developers.Vitor.Couch;
using IngameDebugConsole;
using MadSmith.Scripts.GameSaving;
using MadSmith.Scripts.Utils;
using UnityEngine;
using Utils;

namespace MadSmith.Scripts.Managers
{
    public class SaveGameManager : PersistentSingleton<SaveGameManager>
    {
        public LocalPlayerManager localPlayerManager;

        [Header("Current Game Data")] 
        private int _currentGameSlotIndex;
        private GameSaveData _currentGameSaveData;

        [Header("Save Data Writer")] 
        private SaveFileDataWriter _saveFileDataWriter;

        [Header("Game Save Data Slots")] 
        [SerializeField] private int gameSlotsAmount;
        public GameSaveData[] gameSaveData;
        protected override void Awake()
        {
            base.Awake();
            gameSaveData = new GameSaveData[gameSlotsAmount];
        }

        private void Start()
        {
            LoadAllGameSlots();
        }
        public static string DecideGameDataFileNameBasedOnIndex(int slotIndex)
        {
            return "GameSlot" + slotIndex.ToString().PadLeft(2,'0') + ".txt";
        }

        public GameSaveData GetGameDataSlotByIndex(int index)
        {
            return gameSaveData[index];
        }

        public bool AttemptCreateNewGame()
        {
            _saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath
            };
            for (var index = 0; index < gameSaveData.Length; index++)
            {
                _saveFileDataWriter.saveFileName = DecideGameDataFileNameBasedOnIndex(index);
                if (!_saveFileDataWriter.CheckIfFileExists())
                {
                    _currentGameSlotIndex = index;
                    _currentGameSaveData = new GameSaveData();
                    return true;
                }
            }
            Debug.Log("No slots available");
            return false;
        }

        [ContextMenu("Save Game")]
        [ConsoleMethod("save","Save game on current slot index")]
        public void SaveGame()
        {
            if (ReferenceEquals(_currentGameSaveData, null))
            {
                Debug.LogError("Slot has no data to load. Try NewGame instead.");
                return;
            }
            var saveFileName = DecideGameDataFileNameBasedOnIndex(_currentGameSlotIndex);

            _saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName,
            };
            
            // Fill the GameSaveData
            localPlayerManager.SaveGameToCurrentGameData(ref _currentGameSaveData);
            gameSaveData[_currentGameSlotIndex] = _currentGameSaveData;
            _saveFileDataWriter.CreateNewGameSaveData(_currentGameSaveData);
        }
        
        [ContextMenu("Load Game")]
        public void LoadGame(int slot)
        {
            if (slot < 0 || slot >= gameSaveData.Length || gameSaveData[slot] is not { loaded: true })
            {
                Debug.LogError("Slot has no data to load. Try NewGame instead.");
                return;
            }
            _currentGameSlotIndex = slot;
            _saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = DecideGameDataFileNameBasedOnIndex(_currentGameSlotIndex),
            };
            _currentGameSaveData = _saveFileDataWriter.LoadSaveFile();
            localPlayerManager.LoadDataFromCurrentGameData(ref _currentGameSaveData);
            // Load game scene
        }
        public void ReloadGame()
        {
            LoadGame(_currentGameSlotIndex);
        }

        public void SetCurrentSlot(int newIndex)
        {
            _currentGameSlotIndex = newIndex;
        }

        public void DeleteGame(int indexSlot)
        {
            _saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = DecideGameDataFileNameBasedOnIndex(indexSlot)
            };
            _saveFileDataWriter.DeleteSaveFile();
            gameSaveData[indexSlot] = null;
        }

        private void LoadAllGameSlots()
        {
            _saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath
            };
            for (var index = 0; index < gameSaveData.Length; index++)
            {
                _saveFileDataWriter.saveFileName = DecideGameDataFileNameBasedOnIndex(index);
                gameSaveData[index] = _saveFileDataWriter.LoadSaveFile();
            }
        }
    }
}