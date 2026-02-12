using UnityEngine;

namespace WaterDropSurvival.Systems
{
    /// <summary>
    /// Manages wave progression and difficulty scaling.
    /// Works with EnemySpawner to control game pacing.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Settings")]
        [SerializeField] private float waveDuration = 60f; // 1 minute per wave
        [SerializeField] private float difficultyScaling = 1.1f;
        
        [Header("Boss Waves")]
        [SerializeField] private int bossWaveInterval = 5; // Boss every 5 waves
        [SerializeField] private GameObject bossPrefab;
        
        private int currentWave = 1;
        private float waveTimer = 0f;
        private bool isBossWave = false;
        
        // Events
        public delegate void OnWaveChanged(int waveNumber);
        public event OnWaveChanged WaveChanged;
        
        public int CurrentWave => currentWave;
        
        private void Update()
        {
            waveTimer += Time.deltaTime;
            
            // Check if wave should advance (optional - can also be enemy count based)
            if (waveTimer >= waveDuration)
            {
                AdvanceWave();
            }
        }
        
        private void AdvanceWave()
        {
            currentWave++;
            waveTimer = 0f;
            
            // Check if this is a boss wave
            isBossWave = (currentWave % bossWaveInterval == 0);
            
            if (isBossWave)
            {
                SpawnBoss();
            }
            
            WaveChanged?.Invoke(currentWave);
            Debug.Log($"Wave {currentWave} started! {(isBossWave ? "BOSS WAVE!" : "")}");
        }
        
        private void SpawnBoss()
        {
            if (bossPrefab == null) return;
            
            // Find player to spawn boss near
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 spawnPos = (Vector2)player.transform.position + Random.insideUnitCircle.normalized * 20f;
                Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            }
        }
        
        /// <summary>
        /// Get difficulty multiplier for current wave.
        /// </summary>
        public float GetDifficultyMultiplier()
        {
            return Mathf.Pow(difficultyScaling, currentWave - 1);
        }
        
        /// <summary>
        /// Check if current wave is a boss wave.
        /// </summary>
        public bool IsBossWave()
        {
            return isBossWave;
        }
    }
}
