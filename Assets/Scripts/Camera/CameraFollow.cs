using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float followSpeed = .5f;
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
                var targetPos = new Vector3(transform.position.x, goToFollow.transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed);
            }
        }
    }
}