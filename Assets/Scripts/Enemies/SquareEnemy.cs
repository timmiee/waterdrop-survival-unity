using UnityEngine;

namespace WaterDropSurvival.Enemies
{
    /// <summary>
    /// Square-shaped enemy with basic stats.
    /// Part of the squishy enemy variety.
    /// </summary>
    public class SquareEnemy : EnemyBase
    {
        protected override void Start()
        {
            base.Start();
            
            enemyName = "Square Enemy";
            maxHealth = 100f;
            currentHealth = maxHealth;
            moveSpeed = 2.5f;
            attackDamage = 33f;
            experienceValue = 1;
        }
    }
}
