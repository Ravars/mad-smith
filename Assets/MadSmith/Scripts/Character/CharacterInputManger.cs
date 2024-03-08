using System;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterInputManger : MonoBehaviour
    {
        public Vector2 MovingInputDirection { get; protected set; }
        public Quaternion TargetRotation { get; set; }
        
        [field: Header("Moving")]
        [field: SerializeField] public float SmoothRotation { get; protected set; } = 5f;
        [field: SerializeField] public float MoveSpeed  { get; protected set; } = 5f;

    }
}