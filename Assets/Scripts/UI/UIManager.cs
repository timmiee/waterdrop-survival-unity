using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WaterDropSurvival.Player;

namespace WaterDropSurvival.UI
{
    /// <summary>
    /// Main UI manager that controls all UI elements.
    /// Coordinates between different UI components.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject gameplayUI;
        [SerializeField] private GameObject pauseUI;
        [SerializeField] private GameObject gameOverUI;
        
        [Header("References")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private StatsDisplay statsDisplay;
        [SerializeField] private PlayerStats playerStats;
        
        [Header("Text Elements")]
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI timeText;
        
        private float gameTime = 0f;
        private bool isPaused = false;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindObjectOfType<PlayerStats>();
            }
            
            // Subscribe to player events
            if (playerStats != null)
            {
                playerStats.Death += OnPlayerDeath;
                playerStats.LevelUp += OnPlayerLevelUp;
            }
            
            // Initialize UI
            ShowGameplayUI();
        }
        
        private void OnDestroy()
        {
            if (playerStats != null)
            {
                playerStats.Death -= OnPlayerDeath;
                playerStats.LevelUp -= OnPlayerLevelUp;
            }
        }
        
        private void Update()
        {
            if (!isPaused)
            {
                gameTime += Time.deltaTime;
                UpdateTimeDisplay();
            }
            
            UpdateLevelDisplay();
            UpdateWaveDisplay();
        }
        
        private void UpdateTimeDisplay()
        {
            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(gameTime / 60f);
                int seconds = Mathf.FloorToInt(gameTime % 60f);
                timeText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        
        private void UpdateLevelDisplay()
        {
            if (levelText != null && playerStats != null)
            {
                levelText.text = $"Level {playerStats.CurrentLevel}";
            }
        }
        
        private void UpdateWaveDisplay()
        {
            if (waveText != null)
            {
                var waveManager = FindObjectOfType<Systems.WaveManager>();
                if (waveManager != null)
                {
                    waveText.text = $"Wave {waveManager.CurrentWave}";
                }
            }
        }
        
        public void ShowGameplayUI()
        {
            if (gameplayUI != null) gameplayUI.SetActive(true);
            if (pauseUI != null) pauseUI.SetActive(false);
            if (gameOverUI != null) gameOverUI.SetActive(false);
        }
        
        public void ShowPauseUI()
        {
            isPaused = true;
            Time.timeScale = 0f;
            if (pauseUI != null) pauseUI.SetActive(true);
            if (gameplayUI != null) gameplayUI.SetActive(false);
        }
        
        public void ShowGameOverUI()
        {
            Time.timeScale = 0f;
            if (gameOverUI != null) gameOverUI.SetActive(true);
            if (gameplayUI != null) gameplayUI.SetActive(false);
        }
        
        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
            ShowGameplayUI();
        }
        
        private void OnPlayerDeath()
        {
            ShowGameOverUI();
        }
        
        private void OnPlayerLevelUp(int newLevel)
        {
            // Level up UI is handled by UpgradeSystem
        }
    }
}
