using UnityEngine;

namespace WaterDropSurvival.Player
{
    /// <summary>
    /// Manages player statistics including health, damage, speed, and progression stats.
    /// Based on the starting stats from the game requirements.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;
        
        [Header("Combat Stats")]
        [SerializeField] private float strength = 1f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private float attackSpeed = 1f;
        [SerializeField] private float critChance = 0.1f; // 10%
        [SerializeField] private float critDamage = 1.5f;
        [SerializeField] private float physicalDamage = 1f;
        [SerializeField] private float magicalDamage = 1f;
        
        [Header("Defense Stats")]
        [SerializeField] private float armor = 0f;
        [SerializeField] private float dodge = 0f;
        [SerializeField] private float evasion = 0f;
        
        [Header("Movement Stats")]
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float walkSpeedMultiplier = 0.25f; // 25%
        
        [Header("Regeneration")]
        [SerializeField] private float healthRegen = 0f;
        
        [Header("Level & Experience")]
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int currentExperience = 0;
        [SerializeField] private int experienceToNextLevel = 2;
        
        // Events
        public delegate void OnHealthChanged(float current, float max);
        public event OnHealthChanged HealthChanged;
        
        public delegate void OnLevelUp(int newLevel);
        public event OnLevelUp LevelUp;
        
        public delegate void OnDeath();
        public event OnDeath Death;
        
        // Properties
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public float Strength => strength;
        public float Damage => damage;
        public float AttackSpeed => attackSpeed;
        public float CritChance => critChance;
        public float CritDamage => critDamage;
        public float PhysicalDamage => physicalDamage;
        public float MagicalDamage => magicalDamage;
        public float Armor => armor;
        public float Dodge => dodge;
        public float Evasion => evasion;
        public float MoveSpeed => baseSpeed * (1f + walkSpeedMultiplier);
        public float HealthRegen => healthRegen;
        public int CurrentLevel => currentLevel;
        public int CurrentExperience => currentExperience;
        public int ExperienceToNextLevel => experienceToNextLevel;
        public bool IsAlive => currentHealth > 0;
        
        private void Start()
        {
            currentHealth = maxHealth;
            HealthChanged?.Invoke(currentHealth, maxHealth);
        }
        
        private void Update()
        {
            // Health regeneration
            if (healthRegen > 0 && currentHealth < maxHealth)
            {
                Heal(healthRegen * Time.deltaTime);
            }
        }
        
        /// <summary>
        /// Apply damage to the player, accounting for armor reduction.
        /// </summary>
        public void TakeDamage(float damageAmount)
        {
            if (!IsAlive) return;
            
            // Check dodge/evasion
            if (dodge > 0 && Random.value < dodge)
            {
                Debug.Log("Attack dodged!");
                return;
            }
            
            // Apply armor reduction (armor reduces percentage of damage)
            float damageReduction = armor / (armor + 100f);
            float actualDamage = damageAmount * (1f - damageReduction);
            
            currentHealth = Mathf.Max(0, currentHealth - actualDamage);
            HealthChanged?.Invoke(currentHealth, maxHealth);
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        /// <summary>
        /// Heal the player by a specific amount.
        /// </summary>
        public void Heal(float amount)
        {
            if (!IsAlive) return;
            
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            HealthChanged?.Invoke(currentHealth, maxHealth);
        }
        
        /// <summary>
        /// Add experience and check for level up.
        /// </summary>
        public void AddExperience(int amount)
        {
            currentExperience += amount;
            
            while (currentExperience >= experienceToNextLevel)
            {
                currentExperience -= experienceToNextLevel;
                LevelUpPlayer();
            }
        }
        
        private void LevelUpPlayer()
        {
            currentLevel++;
            // Increase XP requirement (2 kills for level 1, 4 for level 2, etc.)
            experienceToNextLevel = currentLevel * 2;
            
            LevelUp?.Invoke(currentLevel);
        }
        
        /// <summary>
        /// Apply stat upgrades (called from upgrade system).
        /// </summary>
        public void ApplyUpgrade(string upgradeType)
        {
            switch (upgradeType.ToLower())
            {
                case "attack":
                    damage *= 1.1f;
                    physicalDamage *= 1.1f;
                    magicalDamage *= 1.1f;
                    break;
                case "attackspeed":
                    attackSpeed *= 1.1f;
                    break;
                case "armor":
                    armor += 0.25f;
                    break;
                case "health":
                    maxHealth *= 1.1f;
                    currentHealth = maxHealth;
                    HealthChanged?.Invoke(currentHealth, maxHealth);
                    break;
                case "speed":
                    walkSpeedMultiplier += 0.1f;
                    break;
                case "critchance":
                    critChance = Mathf.Min(1f, critChance + 0.05f);
                    break;
                case "critdamage":
                    critDamage += 0.25f;
                    break;
                case "healthregen":
                    healthRegen += 1f;
                    break;
            }
        }
        
        /// <summary>
        /// Calculate final damage including crit chance.
        /// </summary>
        public float CalculateDamage(float baseDamage)
        {
            float totalDamage = baseDamage * damage;
            
            // Check for critical hit
            if (Random.value < critChance)
            {
                totalDamage *= critDamage;
            }
            
            return totalDamage;
        }
        
        private void Die()
        {
            Death?.Invoke();
            Debug.Log("Player died!");
        }
    }
}
