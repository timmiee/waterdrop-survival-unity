using UnityEngine;
using WaterDropSurvival.Player;
using WaterDropSurvival.Systems;
using WaterDropSurvival.Enemies;

namespace WaterDropSurvival
{
    /// <summary>
    /// Main game manager that coordinates all game systems.
    /// Handles game state, initialization, and system references.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Game State")]
        [SerializeField] private GameState currentState = GameState.Playing;
        
        [Header("System References")]
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private ExperienceManager experienceManager;
        [SerializeField] private LevelSystem levelSystem;
        [SerializeField] private UpgradeSystem upgradeSystem;
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private EnemySpawner enemySpawner;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject experiencePickupPrefab;
        
        // Singleton instance
        public static GameManager Instance { get; private set; }
        
        public GameState CurrentState => currentState;
        public PlayerStats PlayerStats => playerStats;
        public ExperienceManager ExperienceManager => experienceManager;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Find or create systems
            InitializeSystems();
        }
        
        private void Start()
        {
            StartGame();
        }
        
        private void InitializeSystems()
        {
            // Auto-find systems if not assigned
            if (playerStats == null)
                playerStats = FindObjectOfType<PlayerStats>();
            
            if (experienceManager == null)
                experienceManager = FindObjectOfType<ExperienceManager>();
            
            if (levelSystem == null)
                levelSystem = FindObjectOfType<LevelSystem>();
            
            if (upgradeSystem == null)
                upgradeSystem = FindObjectOfType<UpgradeSystem>();
            
            if (waveManager == null)
                waveManager = FindObjectOfType<WaveManager>();
            
            if (enemySpawner == null)
                enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        
        private void StartGame()
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            
            // Initialize player if not in scene
            if (playerStats == null && playerPrefab != null)
            {
                GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                playerStats = player.GetComponent<PlayerStats>();
            }
            
            Debug.Log("Game started!");
        }
        
        public void PauseGame()
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
        }
        
        public void ResumeGame()
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
        }
        
        public void GameOver()
        {
            currentState = GameState.GameOver;
            Time.timeScale = 0f;
            Debug.Log("Game Over!");
        }
        
        public void RestartGame()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
        
        /// <summary>
        /// Get the experience pickup prefab for spawning.
        /// </summary>
        public GameObject GetExperiencePickupPrefab()
        {
            return experiencePickupPrefab;
        }
    }
    
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Victory
    }
}
