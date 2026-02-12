using UnityEngine;
using WaterDropSurvival.Player;
using System.Collections.Generic;

namespace WaterDropSurvival.Systems
{
    /// <summary>
    /// Manages player upgrades and presents upgrade choices on level up.
    /// Handles stat upgrade application.
    /// </summary>
    public class UpgradeSystem : MonoBehaviour
    {
        [System.Serializable]
        public class Upgrade
        {
            public string name;
            public string description;
            public string type; // attack, attackspeed, armor, health, speed, etc.
            public Sprite icon;
        }
        
        [Header("Available Upgrades")]
        [SerializeField] private List<Upgrade> allUpgrades = new List<Upgrade>();
        
        [Header("References")]
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private GameObject upgradeMenuUI;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindObjectOfType<PlayerStats>();
            }
            
            if (upgradeMenuUI != null)
            {
                upgradeMenuUI.SetActive(false);
            }
            
            // Initialize default upgrades
            InitializeDefaultUpgrades();
        }
        
        private void InitializeDefaultUpgrades()
        {
            if (allUpgrades.Count == 0)
            {
                allUpgrades.Add(new Upgrade
                {
                    name = "Attack +10%",
                    description = "Increase attack damage by 10%",
                    type = "attack"
                });
                
                allUpgrades.Add(new Upgrade
                {
                    name = "Attack Speed +10%",
                    description = "Increase attack speed by 10%",
                    type = "attackspeed"
                });
                
                allUpgrades.Add(new Upgrade
                {
                    name = "Armor +25%",
                    description = "Increase armor by 25%",
                    type = "armor"
                });
                
                allUpgrades.Add(new Upgrade
                {
                    name = "Health +10%",
                    description = "Increase max health by 10% and fully heal",
                    type = "health"
                });
                
                allUpgrades.Add(new Upgrade
                {
                    name = "Move Speed +10%",
                    description = "Increase movement speed by 10%",
                    type = "speed"
                });
                
                allUpgrades.Add(new Upgrade
                {
                    name = "Crit Chance +5%",
                    description = "Increase critical hit chance by 5%",
                    type = "critchance"
                });
                
                allUpgrades.Add(new Upgrade
                {
                    name = "Crit Damage +25%",
                    description = "Increase critical damage multiplier by 25%",
                    type = "critdamage"
                });
                
                allUpgrades.Add(new Upgrade
                {
                    name = "Health Regen +1",
                    description = "Gain 1 health per second regeneration",
                    type = "healthregen"
                });
            }
        }
        
        /// <summary>
        /// Show the upgrade menu with random choices.
        /// </summary>
        public void ShowUpgradeMenu()
        {
            if (upgradeMenuUI != null)
            {
                upgradeMenuUI.SetActive(true);
            }
            
            // Get 3 random upgrades
            List<Upgrade> choices = GetRandomUpgrades(3);
            
            // Update UI with choices (will be implemented in UI script)
            var upgradeMenu = upgradeMenuUI?.GetComponent<UI.UpgradeMenu>();
            if (upgradeMenu != null)
            {
                upgradeMenu.DisplayUpgrades(choices);
            }
        }
        
        /// <summary>
        /// Hide the upgrade menu and resume game.
        /// </summary>
        public void HideUpgradeMenu()
        {
            if (upgradeMenuUI != null)
            {
                upgradeMenuUI.SetActive(false);
            }
            
            // Resume game
            Time.timeScale = 1f;
        }
        
        /// <summary>
        /// Apply selected upgrade to player.
        /// </summary>
        public void ApplyUpgrade(Upgrade upgrade)
        {
            if (playerStats == null) return;
            
            playerStats.ApplyUpgrade(upgrade.type);
            Debug.Log($"Applied upgrade: {upgrade.name}");
            
            HideUpgradeMenu();
        }
        
        /// <summary>
        /// Get random upgrades for selection.
        /// </summary>
        private List<Upgrade> GetRandomUpgrades(int count)
        {
            List<Upgrade> choices = new List<Upgrade>();
            List<Upgrade> available = new List<Upgrade>(allUpgrades);
            
            for (int i = 0; i < count && available.Count > 0; i++)
            {
                int index = Random.Range(0, available.Count);
                choices.Add(available[index]);
                available.RemoveAt(index);
            }
            
            return choices;
        }
    }
}
