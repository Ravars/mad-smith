using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SimpleMultiplayerPlayer : MonoBehaviour
{
    public PlayerInput playerInput;

    private void Awake()
    {
        // playerInput.onActionTriggered+= PlayerInputOnonActionTriggered;
        // playerInput.get
    }

    private void PlayerInputOnonActionTriggered(InputAction.CallbackContext obj)
    {
    }

    // Just add *some* kind of movement. The specifics here are not of interest. Serves just to
    // demonstrate that the inputs are indeed separate.
    public void OnTeleport()
    {
        transform.position = new Vector3(Random.Range(-75, 75), 0.5f, Random.Range(-75, 75));
    }

    public void OnMove(Vector2 moving)
    {
        Debug.Log("A" + moving);
        
    }
}
