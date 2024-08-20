using System;
using Unity.Netcode;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : NetworkBehaviour
    {
        [SerializeField] private GameObject goToFollow;
        private bool canFollow = false;

        [Rpc(SendTo.Everyone)]
        public void InitCameraFollowRpc()
        {
            this.goToFollow = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
            canFollow = true;
        }

        private void FixedUpdate()
        {
            if(canFollow && goToFollow) 
                transform.position = new Vector3(transform.position.x, goToFollow.transform.position.y, transform.position.z);
        }
    }
}