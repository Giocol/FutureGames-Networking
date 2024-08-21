using System;
using UnityEngine;

namespace Environment
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private float timeToLive = 10;
        private float timeAlive;

        private void Start()
        {
            timeAlive = 0;
        }

        private void Update()
        {
            timeAlive += Time.deltaTime;
            if(timeAlive >= timeToLive)
                Destroy(gameObject);
        }

        private void OnCollisionEnter2D (Collision2D other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();
            if(!player)
                return;
            player.OnTakeDamage();
            
            //play crash sfx, etc.
        }
    }
}