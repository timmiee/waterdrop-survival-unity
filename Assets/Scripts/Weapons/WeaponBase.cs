using UnityEngine;

namespace WaterDropSurvival.Weapons
{
    /// <summary>
    /// Base class for all weapon types in the game.
    /// Handles common weapon functionality like cooldown, damage, and firing.
    /// </summary>
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Weapon Stats")]
        [SerializeField] protected string weaponName;
        [SerializeField] protected float baseDamage = 10f;
        [SerializeField] protected float fireRate = 1f; // Shots per second
        [SerializeField] protected int weaponLevel = 1;
        
        [Header("References")]
        [SerializeField] protected Transform firePoint;
        
        protected float nextFireTime = 0f;
        protected Transform playerTransform;
        
        public string WeaponName => weaponName;
        public float BaseDamage => baseDamage;
        public int WeaponLevel => weaponLevel;
        
        protected virtual void Start()
        {
            playerTransform = transform.root;
        }
        
        protected virtual void Update()
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + (1f / fireRate);
            }
        }
        
        /// <summary>
        /// Fire the weapon. Must be implemented by derived classes.
        /// </summary>
        protected abstract void Fire();
        
        /// <summary>
        /// Upgrade the weapon to the next level.
        /// </summary>
        public virtual void Upgrade()
        {
            weaponLevel++;
            baseDamage *= 1.1f; // 10% damage increase per level
            Debug.Log($"{weaponName} upgraded to level {weaponLevel}");
        }
        
        /// <summary>
        /// Find the nearest enemy target.
        /// </summary>
        protected GameObject FindNearestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject nearest = null;
            float minDistance = Mathf.Infinity;
            
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy;
                }
            }
            
            return nearest;
        }
        
        /// <summary>
        /// Get direction to target.
        /// </summary>
        protected Vector2 GetDirectionToTarget(Transform target)
        {
            if (target == null) return Vector2.right;
            return (target.position - transform.position).normalized;
        }
    }
}
