using UnityEngine;
using WaterDropSurvival.Player;

namespace WaterDropSurvival.Systems
{
    /// <summary>
    /// Manages experience collection and distribution to the player.
    /// Tracks total experience gained.
    /// </summary>
    public class ExperienceManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerStats playerStats;
        
        private int totalExperience = 0;
        
        // Events
        public delegate void OnExperienceGained(int amount, int total);
        public event OnExperienceGained ExperienceGained;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindObjectOfType<PlayerStats>();
            }
        }
        
        /// <summary>
        /// Add experience to the player.
        /// </summary>
        public void AddExperience(int amount)
        {
            if (playerStats == null) return;
            
            totalExperience += amount;
            playerStats.AddExperience(amount);
            
            ExperienceGained?.Invoke(amount, totalExperience);
        }
        
        public int GetTotalExperience()
        {
            return totalExperience;
        }
    }
}
