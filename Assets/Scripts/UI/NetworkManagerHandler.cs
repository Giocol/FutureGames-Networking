using Unity.Netcode;
using UnityEngine;

namespace UI
{
    //Mostly taken from Unity's official NGO docs
    public class NetworkManagerHandler : MonoBehaviour
    {
        private NetworkManager networkManagerRef;

        void Awake()
        {
            networkManagerRef = GetComponent<NetworkManager>();
            
            if(!networkManagerRef)
                Debug.LogError("Missing NetworkManager ref! Please make sure to add a NetworkManager component to the prefab");
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
            if (GUILayout.Button("Host")) networkManagerRef.StartHost();
            if (GUILayout.Button("Client")) networkManagerRef.StartClient();
            if (GUILayout.Button("Server")) networkManagerRef.StartServer();
        }
        void StatusLabels()
        {
            var mode = networkManagerRef.IsHost ?
                "Host" : networkManagerRef.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                            networkManagerRef.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }
        
    }
}
