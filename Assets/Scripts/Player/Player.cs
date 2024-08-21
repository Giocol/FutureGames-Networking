using System;
using Camera;
using Data;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private float movementSpeed = 0.1f;
        [SerializeField] private float forwardSpeed = 1f;
        [SerializeField] private Vector3 hostSpawnPosition;
        [SerializeField] private Vector3 clientSpanwPosition;
        [SerializeField] private GameState gameState;
        
        private InputAction moveAction;
        private PlayerControls controls;
        private Vector3 targetPosition;

        private NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>();

        [SerializeField] private CameraFollow hostCameraFollow;
        [SerializeField] private CameraFollow clientCameraFollow;
        
        [Rpc(SendTo.Everyone)]
        public void OnStartRpc()
        {
            hostCameraFollow.InitCameraFollow(gameObject);
            clientCameraFollow.InitCameraFollow(gameObject);
            gameState.isGameRunning = true;
        }

        public override void OnNetworkSpawn()
        {
            if(IsServer)
                transform.position = IsLocalPlayer ? hostSpawnPosition : clientSpanwPosition;
            base.OnNetworkSpawn();
        }

        public void OnTakeDamage()
        {
            Debug.Log("YOU LOSE!");
            //rpc on loss
        }

        private void Awake()
        {
            controls = new PlayerControls();
            controls.Enable();
            controls.MainPlayer.Move.performed += ctx => OnMove(ctx.ReadValue<Vector2>());
            controls.MainPlayer.Move.canceled += _ => OnMove(new Vector2(0, 0));
            controls.MainPlayer.Shoot.performed += _ => OnShoot();

            if (IsServer)
                transform.position = hostSpawnPosition;
            else
                transform.position = clientSpanwPosition;

            hostCameraFollow = UnityEngine.Camera.allCameras[0].GetComponent<CameraFollow>(); //awful approach, i know :/
            clientCameraFollow = UnityEngine.Camera.allCameras[1].GetComponent<CameraFollow>();
            
            //targetPosition = transform.position;
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

        private void FixedUpdate()
        {
            if (IsServer && gameState.isGameRunning)
            {
                //targetPosition += new Vector3(moveInput.Value.x * movementSpeed, forwardSpeed);
                //transform.position = targetPosition;
                transform.position += new Vector3(moveInput.Value.x * movementSpeed, forwardSpeed);
            }
        }

        [Rpc(SendTo.Server)]
        private void SendInputToServerRPC(Vector2 input)
        {
            moveInput.Value = input;
        }
    }
}