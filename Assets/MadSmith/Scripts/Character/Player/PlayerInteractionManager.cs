using UnityEngine;

namespace MadSmith.Scripts.Character.Player
{
    public class PlayerInteractionManager : CharacterInteractionManager
    {
        public void AttemptPerformGrab()
        {
            Debug.Log("Grab");
        }
        public void AttemptPerformAttack()
        {
            Debug.Log("Attack");
        }
    }
}