using MadSmith.Scripts.Input;
using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerInputManager : CharacterInputManger
    {
        [field:SerializeField] public InputReader InputReader { get; private set; }
        private void OnEnable()
        {
            InputReader.MoveEvent += InputReaderOnMoveEvent;
            InputReader.MoveCanceledEvent += InputReaderOnMoveCanceledEvent;
        }

        private void OnDisable()
        {
            InputReader.MoveEvent -= InputReaderOnMoveEvent;
        }
        public void SetInputReader(InputReader inputReader)
        {
            InputReader = inputReader;
        }
        
        #region Events Subscriptions
        private void InputReaderOnMoveEvent(Vector2 movingInputDirection)
        {
            MovingInputDirection = movingInputDirection;
        }
        private void InputReaderOnMoveCanceledEvent()
        {
            MovingInputDirection = Vector2.zero;
        }

        #endregion
    }
}