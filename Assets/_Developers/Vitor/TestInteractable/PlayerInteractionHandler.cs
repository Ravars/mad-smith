using System;
using UnityEngine;

namespace _Developers.Vitor.TestInteractable
{
    public class PlayerInteractionHandler : MonoBehaviour
    {
        private InputHandler _inputHandler;

        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
        }

        private void OnEnable()
        {
            _inputHandler.InteractEvent += InputHandlerOnInteractEvent;
        }

        private void InputHandlerOnInteractEvent()
        {
            
        }
    }
}