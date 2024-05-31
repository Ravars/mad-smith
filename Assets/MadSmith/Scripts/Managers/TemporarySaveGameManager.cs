using System;
using _Developers.Vitor.Couch;
using MadSmith.Scripts.GameSaving;
using MadSmith.Scripts.Utils;
using UnityEngine;
using Utils;

namespace MadSmith.Scripts.Managers
{
    public class TemporarySaveGameManager : PersistentSingleton<TemporarySaveGameManager>
    {
        public LocalPlayerManager localPlayerManager;
        public bool saveGame = false;
        public bool loadGame = false;


        [Header("Current Game Data")]
        public int currentGameSlotIndex;
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

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void Start()
        {
            LoadAllGameSlots();
        }
        public string DecideGameDataFileNameBasedOnIndex(int slotIndex)
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
                    // saveFileName = DecideGameDataFileNameBasedOnIndex(index);
                    currentGameSlotIndex = index;
                    _currentGameSaveData = new GameSaveData();
                    return true;
                }
            }
            Debug.Log("No slots available");
            return false;
            // saveFileName = DecideGameDataFileNameBasedOnIndex(currentGameSlotIndex);
            // _currentGameSaveData = new GameSaveData();
        }

        [ContextMenu("Save Game")]
        public string SaveGame()
        {
            var saveFileName = DecideGameDataFileNameBasedOnIndex(currentGameSlotIndex);

            _saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName,
            };
            
            // Fill the GameSaveData
            localPlayerManager.SaveGameToCurrentGameData(ref _currentGameSaveData);
            gameSaveData[currentGameSlotIndex] = _currentGameSaveData;
            _saveFileDataWriter.CreateNewGameSaveData(_currentGameSaveData);
            return saveFileName;
        }
        
        [ContextMenu("Load Game")]
        public void LoadGame()
        {
            _saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = DecideGameDataFileNameBasedOnIndex(currentGameSlotIndex),
            };
            _currentGameSaveData = _saveFileDataWriter.LoadSaveFile();
            localPlayerManager.LoadDataFromCurrentGameData(ref _currentGameSaveData);
            // Load game scene
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