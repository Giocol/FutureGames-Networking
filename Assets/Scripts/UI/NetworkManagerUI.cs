using Camera;
using Data;
using Unity.Netcode;
using UnityEngine;

namespace UI
{
    //Mostly taken from Unity's official NGO docs
    public class NetworkManagerUI : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera hostCamera;
        [SerializeField] private UnityEngine.Camera clientCamera;
        [SerializeField] private GameState gameState;
        [SerializeField] private GameObject startButton;

        private NetworkManager networkManagerRef;

        void Awake()
        {
            networkManagerRef = GetComponent<NetworkManager>();
            
            if(!networkManagerRef)
                Debug.LogError("Missing NetworkManager ref! Please make sure to add a NetworkManager component to the prefab");
            
            if(!hostCamera)
                Debug.LogError("Missing hostCamera ref! Please make sure to link HostCamera in the editor!");
            
            if(!clientCamera)
                Debug.LogError("Missing clientCamera ref! Please make sure to link ClientCamera in the editor!");
            
            if(!gameState)
                Debug.LogError("Missing gameState ref! Please make sure to link GameState in the editor!");
            gameState.isGameRunning = false;
            
            if(!startButton)
                Debug.LogError("Missing startButton ref! Please make sure to link StartButton in the editor!");
            
            startButton.SetActive(false);
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!networkManagerRef.IsClient && !networkManagerRef.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }
        void StartButtons()
        {
            if (GUILayout.Button("Host"))
            {
                startButton.SetActive(true);
                networkManagerRef.StartHost();
            }

            if (GUILayout.Button("Client"))
            {
                networkManagerRef.StartClient();
            }
        }
        void StatusLabels()
        {
            var mode = networkManagerRef.IsHost ?
                "Host" : networkManagerRef.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                            networkManagerRef.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        public void OnStartButton()
        {
            hostCamera.GetComponent<CameraFollow>().InitCameraFollow();
            clientCamera.GetComponent<CameraFollow>().InitCameraFollow();
            gameState.isGameRunning = true;
            startButton.SetActive(false);
        }
    }
}
