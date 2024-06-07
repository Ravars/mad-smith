using UnityEngine;

namespace MadSmith.Scripts.Character
{
    public class CharacterInteractionManager : MonoBehaviour
    {
        [SerializeField] protected Transform rightHand;
        [SerializeField] protected Transform positionToReleaseItems;
        public virtual void FixedUpdate()
        {
            
        }
    }
}