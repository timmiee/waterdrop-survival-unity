using UnityEngine;
using System.Collections.Generic;

namespace WaterDropSurvival.Map
{
    /// <summary>
    /// Manages different biomes in the game world.
    /// Each biome has unique visual properties and spawn rates.
    /// </summary>
    public class BiomeManager : MonoBehaviour
    {
        [System.Serializable]
        public class Biome
        {
            public string biomeName;
            public Color groundColor = Color.green;
            public Color ambientColor = Color.white;
            public float enemySpawnMultiplier = 1f;
            public Vector2 center;
            public float radius;
        }
        
        [Header("Biomes")]
        [SerializeField] private List<Biome> biomes = new List<Biome>();
        
        [Header("Current Biome")]
        [SerializeField] private Biome currentBiome;
        
        private Transform playerTransform;
        
        private void Start()
        {
            // Find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            
            // Initialize default biomes if none are set
            if (biomes.Count == 0)
            {
                InitializeDefaultBiomes();
            }
            
            UpdateCurrentBiome();
        }
        
        private void Update()
        {
            if (playerTransform != null)
            {
                UpdateCurrentBiome();
            }
        }
        
        private void InitializeDefaultBiomes()
        {
            // Forest biome
            biomes.Add(new Biome
            {
                biomeName = "Forest",
                groundColor = new Color(0.2f, 0.5f, 0.2f),
                ambientColor = new Color(0.8f, 0.9f, 0.8f),
                enemySpawnMultiplier = 1.2f,
                center = new Vector2(-25f, 25f),
                radius = 20f
            });
            
            // Lake biome
            biomes.Add(new Biome
            {
                biomeName = "Lake",
                groundColor = new Color(0.3f, 0.5f, 0.8f),
                ambientColor = new Color(0.9f, 0.95f, 1f),
                enemySpawnMultiplier = 0.5f,
                center = new Vector2(0f, 0f),
                radius = 15f
            });
            
            // Grassland (default)
            biomes.Add(new Biome
            {
                biomeName = "Grassland",
                groundColor = new Color(0.4f, 0.7f, 0.3f),
                ambientColor = Color.white,
                enemySpawnMultiplier = 1f,
                center = new Vector2(0f, 0f),
                radius = 100f
            });
        }
        
        private void UpdateCurrentBiome()
        {
            if (playerTransform == null) return;
            
            Vector2 playerPos = playerTransform.position;
            Biome newBiome = GetBiomeAtPosition(playerPos);
            
            if (newBiome != currentBiome)
            {
                currentBiome = newBiome;
                OnBiomeChanged(currentBiome);
            }
        }
        
        private Biome GetBiomeAtPosition(Vector2 position)
        {
            // Check all biomes, return the smallest one that contains the position
            Biome closestBiome = null;
            float smallestRadius = float.MaxValue;
            
            foreach (Biome biome in biomes)
            {
                float distance = Vector2.Distance(position, biome.center);
                if (distance <= biome.radius && biome.radius < smallestRadius)
                {
                    closestBiome = biome;
                    smallestRadius = biome.radius;
                }
            }
            
            // Return closest biome or default (grassland)
            return closestBiome ?? (biomes.Count > 0 ? biomes[biomes.Count - 1] : null);
        }
        
        private void OnBiomeChanged(Biome newBiome)
        {
            if (newBiome == null) return;
            
            Debug.Log($"Entered biome: {newBiome.biomeName}");
            
            // Apply biome effects (ambient color, etc.)
            RenderSettings.ambientLight = newBiome.ambientColor;
        }
        
        /// <summary>
        /// Get the current biome the player is in.
        /// </summary>
        public Biome GetCurrentBiome()
        {
            return currentBiome;
        }
        
        /// <summary>
        /// Get enemy spawn multiplier for current biome.
        /// </summary>
        public float GetEnemySpawnMultiplier()
        {
            return currentBiome != null ? currentBiome.enemySpawnMultiplier : 1f;
        }
        
        // Visualize biomes in editor
        private void OnDrawGizmos()
        {
            foreach (Biome biome in biomes)
            {
                Gizmos.color = biome.groundColor;
                Gizmos.DrawWireSphere(biome.center, biome.radius);
            }
        }
    }
}
