using UnityEngine;
using WaterDropSurvival.Player;

namespace WaterDropSurvival.Weapons
{
    /// <summary>
    /// Double Barrel weapon - Unlocked at level 10.
    /// Fires two projectiles in a spread pattern.
    /// </summary>
    public class DoubleBarrel : WeaponBase
    {
        [Header("Double Barrel Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 12f;
        [SerializeField] private float projectileLifetime = 3f;
        [SerializeField] private float spreadAngle = 15f; // Degrees between shots
        [SerializeField] private float maxRange = 20f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip shootSound;
        
        private PlayerStats playerStats;
        
        protected override void Start()
        {
            base.Start();
            weaponName = "Double Barrel";
            baseDamage = 20f;
            fireRate = 0.8f;
            
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
            
            // Calculate base direction
            Vector2 direction = GetDirectionToTarget(target.transform);
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Fire two projectiles with spread
            FireProjectile(baseAngle - spreadAngle / 2f);
            FireProjectile(baseAngle + spreadAngle / 2f);
            
            // Play sound
            if (shootSound != null)
            {
                AudioSource.PlayClipAtPoint(shootSound, transform.position, 0.6f);
            }
        }
        
        private void FireProjectile(float angleDegrees)
        {
            if (projectilePrefab == null) return;
            
            // Calculate direction from angle
            float angleRad = angleDegrees * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            
            // Spawn projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, angleDegrees));
            
            // Set projectile velocity
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
            
            // Set projectile damage
            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj != null && playerStats != null)
            {
                proj.SetDamage(playerStats.CalculateDamage(baseDamage));
            }
            
            // Destroy after lifetime
            Destroy(projectile, projectileLifetime);
        }
    }
}
