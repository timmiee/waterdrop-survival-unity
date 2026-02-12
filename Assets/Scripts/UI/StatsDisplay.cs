using UnityEngine;
using TMPro;
using WaterDropSurvival.Player;

namespace WaterDropSurvival.UI
{
    /// <summary>
    /// Displays player stats in the UI.
    /// Shows detailed information about damage, speed, crit, etc.
    /// </summary>
    public class StatsDisplay : MonoBehaviour
    {
        [Header("Text References")]
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private TextMeshProUGUI attackSpeedText;
        [SerializeField] private TextMeshProUGUI moveSpeedText;
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private TextMeshProUGUI critChanceText;
        [SerializeField] private TextMeshProUGUI critDamageText;
        
        [Header("Player Reference")]
        [SerializeField] private PlayerStats playerStats;
        
        [Header("Update Settings")]
        [SerializeField] private float updateInterval = 0.5f;
        
        private float updateTimer = 0f;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindObjectOfType<PlayerStats>();
            }
        }
        
        private void Update()
        {
            updateTimer += Time.deltaTime;
            
            if (updateTimer >= updateInterval)
            {
                updateTimer = 0f;
                UpdateDisplay();
            }
        }
        
        private void UpdateDisplay()
        {
            if (playerStats == null) return;
            
            if (damageText != null)
                damageText.text = $"DMG: {playerStats.Damage:F1}";
            
            if (attackSpeedText != null)
                attackSpeedText.text = $"ATK SPD: {playerStats.AttackSpeed:F2}x";
            
            if (moveSpeedText != null)
                moveSpeedText.text = $"SPEED: {playerStats.MoveSpeed:F1}";
            
            if (armorText != null)
                armorText.text = $"ARMOR: {playerStats.Armor:F1}";
            
            if (critChanceText != null)
                critChanceText.text = $"CRIT: {(playerStats.CritChance * 100):F0}%";
            
            if (critDamageText != null)
                critDamageText.text = $"CRIT DMG: {playerStats.CritDamage:F1}x";
        }
    }
}
