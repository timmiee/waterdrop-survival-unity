using UnityEngine;
using WaterDropSurvival.Player;

namespace WaterDropSurvival.Weapons
{
    /// <summary>
    /// Gun weapon - Starting weapon that shoots projectiles at enemies.
    /// Auto-targets nearest enemy and fires at 1 shot per second.
    /// </summary>
    public class Gun : WeaponBase
    {
        [Header("Gun Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private float projectileLifetime = 3f;
        [SerializeField] private float maxRange = 15f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip shootSound;
        
        private PlayerStats playerStats;
        
        protected override void Start()
        {
            base.Start();
            weaponName = "Gun";
            baseDamage = 15f;
            fireRate = 1f;
            
            playerStats = GetComponentInParent<PlayerStats>();
        }
        
        protected override void Fire()
        {
            // Find nearest enemy
            GameObject target = FindNearestEnemy();
            if (target == null) return;
            
            // Check if enemy is in range
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > maxRange) return;
            
            // Calculate direction
            Vector2 direction = GetDirectionToTarget(target.transform);
            
            // Spawn projectile
            if (projectilePrefab != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                
                // Set projectile velocity
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                }
                
                // Rotate projectile to face direction
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
                
                // Set projectile damage
                Projectile proj = projectile.GetComponent<Projectile>();
                if (proj != null && playerStats != null)
                {
                    proj.SetDamage(playerStats.CalculateDamage(baseDamage));
                }
                
                // Destroy after lifetime
                Destroy(projectile, projectileLifetime);
            }
            
            // Play sound
            if (shootSound != null)
            {
                AudioSource.PlayClipAtPoint(shootSound, transform.position, 0.5f);
            }
        }
    }
    
    /// <summary>
    /// Projectile component for bullets.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        private float damage = 10f;
        
        public void SetDamage(float dmg)
        {
            damage = dmg;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                // Apply damage to enemy
                var enemy = collision.GetComponent<WaterDropSurvival.Enemies.EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                
                // Destroy projectile
                Destroy(gameObject);
            }
        }
    }
}
