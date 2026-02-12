using UnityEngine;
using WaterDropSurvival.Player;

namespace WaterDropSurvival.Weapons
{
    /// <summary>
    /// Sword weapon - Melee weapon unlocked at level 5.
    /// Performs frontal slash attacks in the player's facing direction.
    /// </summary>
    public class Sword : WeaponBase
    {
        [Header("Sword Settings")]
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackArc = 90f; // Degrees
        [SerializeField] private LayerMask enemyLayer;
        
        [Header("Visual Effects")]
        [SerializeField] private GameObject slashEffect;
        [SerializeField] private float effectDuration = 0.3f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip slashSound;
        
        private PlayerStats playerStats;
        private PlayerController playerController;
        
        protected override void Start()
        {
            base.Start();
            weaponName = "Sword";
            baseDamage = 25f;
            fireRate = 1.5f;
            
            playerStats = GetComponentInParent<PlayerStats>();
            playerController = GetComponentInParent<PlayerController>();
        }
        
        protected override void Fire()
        {
            // Get player's facing direction
            Vector2 attackDirection = playerController != null ? 
                playerController.LastMoveDirection : Vector2.right;
            
            if (attackDirection == Vector2.zero)
                attackDirection = Vector2.right;
            
            // Perform slash attack
            PerformSlashAttack(attackDirection);
            
            // Spawn visual effect
            if (slashEffect != null)
            {
                float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
                GameObject effect = Instantiate(slashEffect, transform.position, Quaternion.Euler(0, 0, angle));
                Destroy(effect, effectDuration);
            }
            
            // Play sound
            if (slashSound != null)
            {
                AudioSource.PlayClipAtPoint(slashSound, transform.position, 0.7f);
            }
        }
        
        private void PerformSlashAttack(Vector2 direction)
        {
            // Find all enemies in range
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    // Check if enemy is within attack arc
                    Vector2 toEnemy = (hit.transform.position - transform.position).normalized;
                    float angle = Vector2.Angle(direction, toEnemy);
                    
                    if (angle <= attackArc / 2f)
                    {
                        // Apply damage
                        var enemy = hit.GetComponent<WaterDropSurvival.Enemies.EnemyBase>();
                        if (enemy != null && playerStats != null)
                        {
                            enemy.TakeDamage(playerStats.CalculateDamage(baseDamage));
                        }
                    }
                }
            }
        }
        
        // Visualize attack range in editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
