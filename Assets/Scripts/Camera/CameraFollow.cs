using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        private GameObject goToFollow;
        private bool canFollow = false;
        
        public void InitCameraFollow(GameObject goToFollow)
        {
            this.goToFollow = goToFollow;
            canFollow = true;
        }

        private void FixedUpdate()
        {
            if (canFollow && goToFollow)
            {
                transform.position = new Vector3(transform.position.x, goToFollow.transform.position.y,
                    transform.position.z);
            }
        }
    }
}