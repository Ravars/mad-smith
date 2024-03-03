using System;
using CouchCoop;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Developers.Vitor.TestCouchCoop
{
    public class PlayerMovement : MonoBehaviour
    {
        private CouchInput input;
        [SerializeField] private float speed = 3;
        
        private void Awake()
        {
            input = new CouchInput();
            input.Enable();
            input.Gameplay.Move.performed += MoveOnPerformed;
        }

        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            Vector2 data = obj.ReadValue<Vector2>();
            Vector3 dir = new Vector3(data.x, 0, data.y);
            transform.position += dir * speed * Time.deltaTime;
        }

        // // Start is called before the first frame update
        // void Start()
        // {
        //     
        // }
        //
        // // Update is called once per frame
        // void Update()
        // {
        //     
        // }
    }
}
