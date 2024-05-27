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

        private void DecideGameDataFileNameBasedOnSlotBeingUsed()
        {
            saveFileName = currentGameSaveSlot.ToString() + ".txt";
        }

        public void CreateNewGame()
        {
            DecideGameDataFileNameBasedOnSlotBeingUsed();
            currentGameSaveData = new GameSaveData();
        }

        [ContextMenu("Load Game")]
        public void LoadGame()
        {
            DecideGameDataFileNameBasedOnSlotBeingUsed();
            _saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName,
            };
            currentGameSaveData = _saveFileDataWriter.LoadSaveFile();
            // Load game scene
        }

        [ContextMenu("Save Game")]
        public void SaveGame()
        {
            DecideGameDataFileNameBasedOnSlotBeingUsed();

            _saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName,
            };
            
            // Fill the GameSaveData
            localPlayerManager.SaveGameToCurrentGameData(ref currentGameSaveData);
            
            _saveFileDataWriter.CreateNewGameSaveData(currentGameSaveData);
        }
    }
}