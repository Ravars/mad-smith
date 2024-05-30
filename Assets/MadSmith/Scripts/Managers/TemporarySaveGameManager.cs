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
        public GameDataSlot currentGameSaveSlot;
        public GameSaveData currentGameSaveData;
        [SerializeField] private string saveFileName;

        [Header("Save Data Writer")] 
        private SaveFileDataWriter _saveFileDataWriter;
        
        [Header("Game Save Data Slots")] 
        public GameSaveData gameSlot01;
        public GameSaveData gameSlot02;
        public GameSaveData gameSlot03;
        public GameSaveData gameSlot04;

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

        public string DecideGameDataFileNameBasedOnSlotBeingUsed(GameDataSlot slot)
        {
            return slot.ToString() + ".txt";
        }

        public GameSaveData GetGameDataSlotByName(GameDataSlot slot)
        {
            switch (slot)
            {
                case GameDataSlot.GameSlot01:
                    return gameSlot01;
                case GameDataSlot.GameSlot02:
                    return gameSlot02;
                case GameDataSlot.GameSlot03:
                    return gameSlot03;
                case GameDataSlot.GameSlot04:
                    return gameSlot04;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        public void CreateNewGame()
        {
            saveFileName = DecideGameDataFileNameBasedOnSlotBeingUsed(currentGameSaveSlot);
            currentGameSaveData = new GameSaveData();
        }

        [ContextMenu("Load Game")]
        public void LoadGame()
        {
            saveFileName = DecideGameDataFileNameBasedOnSlotBeingUsed(currentGameSaveSlot);
            _saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName,
            };
            currentGameSaveData = _saveFileDataWriter.LoadSaveFile();
            localPlayerManager.LoadDataFromCurrentGameData(ref currentGameSaveData);
            // Load game scene
        }

        [ContextMenu("Save Game")]
        public string SaveGame()
        {
            saveFileName = DecideGameDataFileNameBasedOnSlotBeingUsed(currentGameSaveSlot);

            _saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName,
            };
            
            // Fill the GameSaveData
            localPlayerManager.SaveGameToCurrentGameData(ref currentGameSaveData);
            
            _saveFileDataWriter.CreateNewGameSaveData(currentGameSaveData);
            return saveFileName;
        }

        private void LoadAllGameSlots()
        {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            _saveFileDataWriter.saveFileName = DecideGameDataFileNameBasedOnSlotBeingUsed(GameDataSlot.GameSlot01);
            gameSlot01 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.saveFileName = DecideGameDataFileNameBasedOnSlotBeingUsed(GameDataSlot.GameSlot02);
            gameSlot02 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.saveFileName = DecideGameDataFileNameBasedOnSlotBeingUsed(GameDataSlot.GameSlot03);
            gameSlot03 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.saveFileName = DecideGameDataFileNameBasedOnSlotBeingUsed(GameDataSlot.GameSlot04);
            gameSlot04 = _saveFileDataWriter.LoadSaveFile();
            
        }
    }
}