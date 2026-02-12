using UnityEngine;
using WaterDropSurvival.Player;

namespace WaterDropSurvival.Systems
{
    /// <summary>
    /// Manages player level progression and level-up events.
    /// Triggers upgrade menu when player levels up.
    /// </summary>
    public class LevelSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private UpgradeSystem upgradeSystem;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindObjectOfType<PlayerStats>();
            }
            
            if (upgradeSystem == null)
            {
                upgradeSystem = FindObjectOfType<UpgradeSystem>();
            }
            
            // Subscribe to level up event
            if (playerStats != null)
            {
                playerStats.LevelUp += OnPlayerLevelUp;
            }
        }
        
        private void OnDestroy()
        {
            if (playerStats != null)
            {
                playerStats.LevelUp -= OnPlayerLevelUp;
            }
        }
        
        private void OnPlayerLevelUp(int newLevel)
        {
            Debug.Log($"Player reached level {newLevel}!");
            
            // Pause game and show upgrade menu
            Time.timeScale = 0f;
            
            if (upgradeSystem != null)
            {
                upgradeSystem.ShowUpgradeMenu();
            }
            
            // Check for weapon unlocks
            CheckWeaponUnlocks(newLevel);
        }
        
        private void CheckWeaponUnlocks(int level)
        {
            if (level == 5)
            {
                Debug.Log("Sword unlocked!");
                // TODO: Unlock sword weapon
            }
            else if (level == 10)
            {
                Debug.Log("Double Barrel and Energy Aura unlocked!");
                // TODO: Unlock double barrel and energy aura
            }
        }
    }
}
