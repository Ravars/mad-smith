using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Bool Event Channel")]
    public class BoolEventChannelSo : ScriptableObject
    {
        public event UnityAction<bool> OnEventRaised;

        public void RaiseEvent(bool value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }
}