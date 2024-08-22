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
        [SerializeField] private GameObject clientButton;
        [SerializeField] private GameObject hostButton;

        private NetworkManager networkManagerRef;
        
        public void OnHostButtonPress()
        {
            networkManagerRef.StartHost();
            
            startButton.SetActive(true);
            ToggleNetworkButtons(false);
        }

        public void OnClientButtonPress()
        {
            networkManagerRef.StartClient();
            
            ToggleNetworkButtons(false);
        }
        
        public void OnStartButton()
        {
            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                client.PlayerObject.gameObject.GetComponent<Player.Player>().OnStartRpc();
            }
            gameState.isGameRunning = true;
            startButton.SetActive(false);
        }
        
        private void Awake()
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
            
            if(!hostButton)
                Debug.LogError("Missing hostButton ref! Please make sure to link hostButton in the editor!");
            
            if(!clientButton)
                Debug.LogError("Missing clientButton ref! Please make sure to link clientButton in the editor!");
            
            
            startButton.SetActive(false);
            ToggleNetworkButtons(true);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (networkManagerRef.IsClient || networkManagerRef.IsServer)
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        private void StatusLabels()
        {
            var mode = networkManagerRef.IsHost ?
                "Host" : networkManagerRef.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                            networkManagerRef.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        private void ToggleNetworkButtons(bool state)
        {
            hostButton.SetActive(state);
            clientButton.SetActive(state);
        }
    }
}
