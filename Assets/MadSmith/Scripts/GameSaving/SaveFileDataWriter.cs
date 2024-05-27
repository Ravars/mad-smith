using System;
using System.IO;
using UnityEngine;

namespace MadSmith.Scripts.GameSaving
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        public bool CheckIfFileExists()
        {
            return File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        public void CreateNewGameSaveData(GameSaveData gameSaveData)
        {
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("Creating save file, at path: " + savePath);

                string dataToStore = JsonUtility.ToJson(gameSaveData, true);
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error trying to save game data, game not saved");
            }
        }

        public GameSaveData LoadSaveFile()
        {
            GameSaveData gameSaveData = null;
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);
            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    gameSaveData = JsonUtility.FromJson<GameSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error trying to Load save file");
                }
            }

            return gameSaveData;
        }
        
    }
}