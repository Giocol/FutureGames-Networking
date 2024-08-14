using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : NetworkBehaviour
    {
        [SerializeField] private float movementSpeed = 0.001f;
        
        private InputAction moveAction;
        private PlayerControls controls;

        private NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>();

        private void Awake()
        {
            controls = new PlayerControls();
            controls.Enable();
            controls.MainPlayer.Move.performed += ctx => OnMove(ctx.ReadValue<Vector2>());
            controls.MainPlayer.Move.canceled += _ => OnMove(new Vector2(0, 0));
            controls.MainPlayer.Shoot.performed += _ => OnShoot();
        }

        private void OnMove(Vector2 input)
        {
            Debug.Log(input);
            if(IsLocalPlayer)
                SendInputToServerRPC(input);
        }

        private void OnShoot()
        {
            Debug.Log("Shoot");
        }

        private void Update()
        {
            if(IsServer) 
                transform.position += (new Vector3(moveInput.Value.x, 0, moveInput.Value.y) * movementSpeed);
        }

        [Rpc(SendTo.Server)]
        private void SendInputToServerRPC(Vector2 input)
        {
            moveInput.Value = input;
        }

    }
}