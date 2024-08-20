using System;
using Unity.Netcode;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private NetworkObject goToFollow;
        private bool canFollow = false;

        public void InitCameraFollow()
        {
            goToFollow = NetworkManager.Singleton.ConnectedClients[0].PlayerObject;
            canFollow = true;
        }

        private void FixedUpdate()
        {
            if(canFollow && goToFollow) 
                transform.position = new Vector3(transform.position.x, goToFollow.transform.position.y, transform.position.z);
        }
    }
}