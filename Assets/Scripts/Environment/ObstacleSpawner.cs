using System;
using Data;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class ObstacleSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private float deltaXMin = 4;
        [SerializeField] private float deltaXMax = 4;
        [SerializeField] private float deltaY = 5;
        [SerializeField] private float spawnCooldown = 5;
        [SerializeField] private GameState gameState;

        private float timeSinceLastSpawn = 0;

        private void Awake()
        {
            if(!obstaclePrefab)
                Debug.LogError("Missing obstacle prefab! Please plug it into the inspector!");
            
            if(!gameState)
                Debug.LogError("Missing gameState! Please plug it into the inspector!");
        }

        private void Update()
        {
            if(!gameState.isGameRunning)
                return;
            
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= spawnCooldown)
            {
                if(IsServer)
                    SpawnObstacle();
                timeSinceLastSpawn = 0;
            }
        }

        private void SpawnObstacle()
        {
            Vector3 position = gameObject.transform.position;
            float spawnX = Random.Range(position.x - deltaXMin, position.x + deltaXMax);
            
            var instance = Instantiate(obstaclePrefab, new Vector3(spawnX, position.y + deltaY, 0), Quaternion.identity);
            var instanceNetworkObject = instance.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();
        }
    }
}