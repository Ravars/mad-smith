using MadSmith.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace MadSmith.Scripts.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private InputReader[] inputReaders;
        
        public InputReader GetInputReader(int index)
        {
            return inputReaders[index];
        }
    }
}