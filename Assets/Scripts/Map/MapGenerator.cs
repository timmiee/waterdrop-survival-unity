using UnityEngine;
using System.Collections.Generic;

namespace WaterDropSurvival.Map
{
    /// <summary>
    /// Generates or manages the game map with multiple biomes.
    /// Creates a large open world with different environmental areas.
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        [Header("Map Settings")]
        [SerializeField] private float mapWidth = 100f;
        [SerializeField] private float mapHeight = 100f;
        [SerializeField] private GameObject groundPrefab;
        
        [Header("Biome Prefabs")]
        [SerializeField] private GameObject forestBiomePrefab;
        [SerializeField] private GameObject lakeBiomePrefab;
        [SerializeField] private GameObject cabinPrefab;
        [SerializeField] private GameObject windmillPrefab;
        [SerializeField] private GameObject minePrefab;
        
        [Header("Environment Objects")]
        [SerializeField] private GameObject[] treePrefabs;
        [SerializeField] private GameObject roadPrefab;
        
        [Header("Generation Settings")]
        [SerializeField] private bool generateOnStart = true;
        [SerializeField] private int treeCount = 50;
        
        private List<GameObject> mapObjects = new List<GameObject>();
        
        private void Start()
        {
            if (generateOnStart)
            {
                GenerateMap();
            }
        }
        
        /// <summary>
        /// Generate the complete map with all biomes and objects.
        /// </summary>
        public void GenerateMap()
        {
            Debug.Log("Generating map...");
            
            // Generate ground
            GenerateGround();
            
            // Generate biomes
            GenerateBiomes();
            
            // Generate environment objects
            GenerateEnvironmentObjects();
            
            Debug.Log("Map generation complete!");
        }
        
        private void GenerateGround()
        {
            if (groundPrefab == null) return;
            
            // Create a large ground plane
            GameObject ground = Instantiate(groundPrefab, Vector3.zero, Quaternion.identity, transform);
            ground.transform.localScale = new Vector3(mapWidth / 10f, mapHeight / 10f, 1f);
            ground.name = "Ground";
            mapObjects.Add(ground);
        }
        
        private void GenerateBiomes()
        {
            // Forest biome (upper left)
            if (forestBiomePrefab != null)
            {
                Vector3 forestPos = new Vector3(-mapWidth * 0.25f, mapHeight * 0.25f, 0);
                GameObject forest = Instantiate(forestBiomePrefab, forestPos, Quaternion.identity, transform);
                forest.name = "Forest Biome";
                mapObjects.Add(forest);
            }
            
            // Lake biome (center)
            if (lakeBiomePrefab != null)
            {
                Vector3 lakePos = new Vector3(0, 0, 0);
                GameObject lake = Instantiate(lakeBiomePrefab, lakePos, Quaternion.identity, transform);
                lake.name = "Lake Biome";
                mapObjects.Add(lake);
            }
            
            // Cabin (upper right)
            if (cabinPrefab != null)
            {
                Vector3 cabinPos = new Vector3(mapWidth * 0.3f, mapHeight * 0.3f, 0);
                GameObject cabin = Instantiate(cabinPrefab, cabinPos, Quaternion.identity, transform);
                cabin.name = "Cabin";
                mapObjects.Add(cabin);
            }
            
            // Windmill (lower right)
            if (windmillPrefab != null)
            {
                Vector3 windmillPos = new Vector3(mapWidth * 0.3f, -mapHeight * 0.3f, 0);
                GameObject windmill = Instantiate(windmillPrefab, windmillPos, Quaternion.identity, transform);
                windmill.name = "Windmill";
                mapObjects.Add(windmill);
            }
            
            // Mine (lower left)
            if (minePrefab != null)
            {
                Vector3 minePos = new Vector3(-mapWidth * 0.3f, -mapHeight * 0.3f, 0);
                GameObject mine = Instantiate(minePrefab, minePos, Quaternion.identity, transform);
                mine.name = "Mine";
                mapObjects.Add(mine);
            }
        }
        
        private void GenerateEnvironmentObjects()
        {
            // Generate trees randomly across the map
            if (treePrefabs != null && treePrefabs.Length > 0)
            {
                for (int i = 0; i < treeCount; i++)
                {
                    Vector3 treePos = new Vector3(
                        Random.Range(-mapWidth * 0.4f, mapWidth * 0.4f),
                        Random.Range(-mapHeight * 0.4f, mapHeight * 0.4f),
                        0
                    );
                    
                    GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                    GameObject tree = Instantiate(treePrefab, treePos, Quaternion.identity, transform);
                    tree.name = $"Tree_{i}";
                    mapObjects.Add(tree);
                }
            }
        }
        
        /// <summary>
        /// Clear all generated map objects.
        /// </summary>
        public void ClearMap()
        {
            foreach (GameObject obj in mapObjects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            mapObjects.Clear();
        }
        
        // Visualize map bounds in editor
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(mapWidth, mapHeight, 0));
        }
    }
}
