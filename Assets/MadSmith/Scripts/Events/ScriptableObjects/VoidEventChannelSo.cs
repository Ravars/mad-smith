using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have no argument.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSo : ScriptableObject
    {
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}