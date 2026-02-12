using UnityEngine;
using System.Collections.Generic;

namespace WaterDropSurvival.Enemies
{
    /// <summary>
    /// Manages wave-based enemy spawning around the player.
    /// Spawns enemies in increasing difficulty waves.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private float spawnRadius = 15f;
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private int initialEnemiesPerWave = 5;
        [SerializeField] private float waveScaling = 1.2f;
        
        [Header("Enemy Prefabs")]
        [SerializeField] private GameObject squareEnemyPrefab;
        [SerializeField] private GameObject triangleEnemyPrefab;
        [SerializeField] private GameObject roundEnemyPrefab;
        
        [Header("Spawn Limits")]
        [SerializeField] private int maxActiveEnemies = 50;
        
        private Transform playerTransform;
        private float nextSpawnTime = 0f;
        private int currentWave = 1;
        private int enemiesSpawnedThisWave = 0;
        private int enemiesPerWave;
        private List<GameObject> activeEnemies = new List<GameObject>();
        
        private void Start()
        {
            // Find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            
            enemiesPerWave = initialEnemiesPerWave;
        }
        
        private void Update()
        {
            if (playerTransform == null) return;
            
            // Clean up destroyed enemies from list
            activeEnemies.RemoveAll(enemy => enemy == null);
            
            // Check if we should spawn more enemies
            if (Time.time >= nextSpawnTime && activeEnemies.Count < maxActiveEnemies)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnInterval;
                
                enemiesSpawnedThisWave++;
                
                // Check if wave is complete
                if (enemiesSpawnedThisWave >= enemiesPerWave)
                {
                    StartNextWave();
                }
            }
        }
        
        private void SpawnEnemy()
        {
            // Get random position around player
            Vector2 spawnPosition = GetRandomSpawnPosition();
            
            // Select random enemy type based on wave
            GameObject enemyPrefab = SelectEnemyType();
            
            if (enemyPrefab != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                activeEnemies.Add(enemy);
            }
        }
        
        private Vector2 GetRandomSpawnPosition()
        {
            // Spawn enemies in a ring around the player
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = spawnRadius + Random.Range(0f, 2f);
            
            Vector2 offset = new Vector2(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance
            );
            
            return (Vector2)playerTransform.position + offset;
        }
        
        private GameObject SelectEnemyType()
        {
            // Weight enemy types based on wave
            float roll = Random.value;
            
            if (currentWave < 3)
            {
                // Early game: mostly squares
                if (roll < 0.7f)
                    return squareEnemyPrefab;
                else if (roll < 0.9f)
                    return triangleEnemyPrefab;
                else
                    return roundEnemyPrefab;
            }
            else if (currentWave < 6)
            {
                // Mid game: balanced mix
                if (roll < 0.4f)
                    return squareEnemyPrefab;
                else if (roll < 0.7f)
                    return triangleEnemyPrefab;
                else
                    return roundEnemyPrefab;
            }
            else
            {
                // Late game: more difficult enemies
                if (roll < 0.3f)
                    return squareEnemyPrefab;
                else if (roll < 0.5f)
                    return triangleEnemyPrefab;
                else
                    return roundEnemyPrefab;
            }
        }
        
        private void StartNextWave()
        {
            currentWave++;
            enemiesSpawnedThisWave = 0;
            enemiesPerWave = Mathf.RoundToInt(initialEnemiesPerWave * Mathf.Pow(waveScaling, currentWave - 1));
            
            // Decrease spawn interval slightly each wave (but not too fast)
            spawnInterval = Mathf.Max(0.5f, spawnInterval * 0.95f);
            
            Debug.Log($"Wave {currentWave} started! Enemies: {enemiesPerWave}");
        }
        
        /// <summary>
        /// Get current wave number for UI display.
        /// </summary>
        public int GetCurrentWave()
        {
            return currentWave;
        }
        
        /// <summary>
        /// Get number of active enemies.
        /// </summary>
        public int GetActiveEnemyCount()
        {
            return activeEnemies.Count;
        }
        
        // Visualize spawn radius in editor
        private void OnDrawGizmosSelected()
        {
            if (playerTransform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, spawnRadius);
            }
        }
    }
}
