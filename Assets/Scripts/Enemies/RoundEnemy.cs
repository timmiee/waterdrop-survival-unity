using UnityEngine;

namespace WaterDropSurvival.Enemies
{
    /// <summary>
    /// Round-shaped enemy with balanced stats.
    /// Part of the squishy enemy variety.
    /// </summary>
    public class RoundEnemy : EnemyBase
    {
        protected override void Start()
        {
            base.Start();
            
            enemyName = "Round Enemy";
            maxHealth = 120f;
            currentHealth = maxHealth;
            moveSpeed = 2f; // Slower but tankier
            attackDamage = 35f;
            experienceValue = 2;
        }
    }
}
