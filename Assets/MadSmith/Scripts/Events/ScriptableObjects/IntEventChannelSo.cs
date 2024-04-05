using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have one int argument.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Int Event Channel")]
    public class IntEventChannelSo : ScriptableObject
    {
        public UnityAction<int> OnEventRaised;

        public void RaiseEvent(int value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}