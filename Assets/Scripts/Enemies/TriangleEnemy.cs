using UnityEngine;

namespace WaterDropSurvival.Enemies
{
    /// <summary>
    /// Triangle-shaped enemy with slightly higher speed.
    /// Part of the squishy enemy variety.
    /// </summary>
    public class TriangleEnemy : EnemyBase
    {
        protected override void Start()
        {
            base.Start();
            
            enemyName = "Triangle Enemy";
            maxHealth = 80f;
            currentHealth = maxHealth;
            moveSpeed = 3f; // Faster than square
            attackDamage = 30f;
            experienceValue = 1;
        }
    }
}
