using UnityEngine;

namespace MadSmith.Scripts.GameSaving
{
    [System.Serializable]
    public class GameSaveData
    {
        [Header("Player Name")]
        public string playerName;

        [Header("Time Played")] 
        public float secondsPlayed;

        public bool loaded = false;

    }
}