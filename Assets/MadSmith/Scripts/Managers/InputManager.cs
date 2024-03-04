using MadSmith.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace MadSmith.Scripts.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private InputReader[] inputReaders;
        protected override void Awake()
        {
            base.Awake();
            foreach (var inputReader in inputReaders)
            {
                inputReader.deviceId = -1;
            }
        }

        public InputReader GetInputReader(int index)
        {
            return inputReaders[index];
        }
    }
}