using System;
using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterInputManger : MonoBehaviour
    {
        // [HideInInspector] 
        [field: SerializeField] public Vector2 MovingInputDirection { get; protected set; }
        // [HideInInspector] 
        [field: SerializeField] public Quaternion TargetRotation { get; set; }
        
        [field: Header("Moving")]
        [field: SerializeField] public float SmoothRotation { get; protected set; } = 5f;
        [field: SerializeField] public float MoveSpeed  { get; protected set; } = 5f;

        protected virtual void Awake(){}

        protected virtual void Update(){}
    }
}