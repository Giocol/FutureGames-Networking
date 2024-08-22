using System;
using Camera;
using Data;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private float movementSpeed = 0.1f;
        [SerializeField] private float forwardSpeed = 1f;
        [SerializeField] private Vector3 hostSpawnPosition;
        [SerializeField] private Vector3 clientSpawnPosition;
        [SerializeField] private GameState gameState;
        [SerializeField] private GameObject clientWaitingUI;
        [SerializeField] private GameObject winUI;
        [SerializeField] private GameObject loseUI;
        
        private InputAction moveAction;
        private PlayerControls controls;
        private Vector3 targetPosition;

        private NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>();

        private CameraFollow hostCameraFollow;
        private CameraFollow clientCameraFollow;
        
        [Rpc(SendTo.Everyone)]
        public void OnStartRpc()
        {
            hostCameraFollow.InitCameraFollow(gameObject);
            clientCameraFollow.InitCameraFollow(gameObject);
            clientWaitingUI.SetActive(false);
            gameState.isGameRunning = true;
        }

        public override void OnNetworkSpawn()
        {
            if(IsServer)
                transform.position = IsLocalPlayer ? hostSpawnPosition : clientSpawnPosition;
            
            if(!clientWaitingUI)
                Debug.LogError("Missing clientWaitingUI ref! Please make sure to link clientWaitingUI in the editor!");
            
            if(!winUI)
                Debug.LogError("Missing winUI ref! Please make sure to link winUI in the editor!");
            
            if(!loseUI)
                Debug.LogError("Missing loseUI ref! Please make sure to link loseUI in the editor!");
            
            clientWaitingUI.SetActive(!IsServer);
            winUI.SetActive(false);
            loseUI.SetActive(false);
            
            base.OnNetworkSpawn();
        }

        public void OnTakeDamage()
        {
            if (gameState.isGameRunning && IsLocalPlayer)
            {
                OnEndGameRpc();
                OnLossRpc();
                OnWinRpc();
            }
        }

        [Rpc(SendTo.Me)]
        private void OnLossRpc()
        {
            loseUI.SetActive(true);
        }
        
        [Rpc(SendTo.NotMe)]
        private void OnWinRpc()
        {
            winUI.SetActive(true);
        }
        
        [Rpc(SendTo.Server)]
        private void OnEndGameRpc()
        {
            gameState.isGameRunning = false;
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
                transform.position = clientSpawnPosition;

            hostCameraFollow = UnityEngine.Camera.allCameras[0].GetComponent<CameraFollow>(); //awful approach, i know :/
            clientCameraFollow = UnityEngine.Camera.allCameras[1].GetComponent<CameraFollow>();
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