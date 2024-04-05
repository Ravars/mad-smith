using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have one float argument.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Float Event Channel")]
    public class FloatEventChannelSo : ScriptableObject
    {
        public UnityAction<float> OnEventRaised;

        public void RaiseEvent(float value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}