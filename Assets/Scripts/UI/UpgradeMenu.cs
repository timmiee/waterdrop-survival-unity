using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using WaterDropSurvival.Systems;

namespace WaterDropSurvival.UI
{
    /// <summary>
    /// Displays upgrade choices when player levels up.
    /// Allows player to select from available upgrades.
    /// </summary>
    public class UpgradeMenu : MonoBehaviour
    {
        [Header("Upgrade Buttons")]
        [SerializeField] private Button[] upgradeButtons;
        [SerializeField] private TextMeshProUGUI[] upgradeNames;
        [SerializeField] private TextMeshProUGUI[] upgradeDescriptions;
        [SerializeField] private Image[] upgradeIcons;
        
        [Header("References")]
        [SerializeField] private UpgradeSystem upgradeSystem;
        
        private List<UpgradeSystem.Upgrade> currentChoices;
        
        private void Start()
        {
            if (upgradeSystem == null)
            {
                upgradeSystem = FindObjectOfType<UpgradeSystem>();
            }
            
            // Setup button listeners
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                int index = i; // Capture for closure
                upgradeButtons[i].onClick.AddListener(() => OnUpgradeSelected(index));
            }
        }
        
        /// <summary>
        /// Display available upgrades to the player.
        /// </summary>
        public void DisplayUpgrades(List<UpgradeSystem.Upgrade> upgrades)
        {
            currentChoices = upgrades;
            
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                if (i < upgrades.Count)
                {
                    // Show upgrade option
                    upgradeButtons[i].gameObject.SetActive(true);
                    
                    if (upgradeNames[i] != null)
                        upgradeNames[i].text = upgrades[i].name;
                    
                    if (upgradeDescriptions[i] != null)
                        upgradeDescriptions[i].text = upgrades[i].description;
                    
                    if (upgradeIcons[i] != null && upgrades[i].icon != null)
                        upgradeIcons[i].sprite = upgrades[i].icon;
                }
                else
                {
                    // Hide unused buttons
                    upgradeButtons[i].gameObject.SetActive(false);
                }
            }
        }
        
        private void OnUpgradeSelected(int index)
        {
            if (currentChoices == null || index >= currentChoices.Count) return;
            
            UpgradeSystem.Upgrade selectedUpgrade = currentChoices[index];
            
            if (upgradeSystem != null)
            {
                upgradeSystem.ApplyUpgrade(selectedUpgrade);
            }
        }
    }
}
