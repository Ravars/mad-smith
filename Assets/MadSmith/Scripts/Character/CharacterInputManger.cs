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

        protected virtual void Awake(){}

        protected virtual void Update(){}
    }
}