using System;
using Data;
using Unity.Netcode;
using UnityEngine;

namespace Environment
{
    public class DamageDealer : NetworkBehaviour
    {
        [SerializeField] private float timeToLive = 10;
        [SerializeField] private GameState gameState;
        
        private float timeAlive;

        private void Start()
        {
            timeAlive = 0;
            if(!gameState)
                Debug.LogError("Missing gameState ref! Please make sure to link GameState in the editor!");
        }

        private void Update()
        {
            timeAlive += Time.deltaTime;
            if(timeAlive >= timeToLive)
                DestroyRpc();
        }

        private void OnCollisionEnter2D (Collision2D other)
        {
            if (!gameState.isGameRunning)
                return;
            
            Player.Player player = other.gameObject.GetComponent<Player.Player>();
            if(!player)
                return;
            player.OnTakeDamage();
            
            //play crash sfx, etc.
        }

        [Rpc(SendTo.Server)]
        private void DestroyRpc()
        {
            Destroy(gameObject);
        }
    }
}