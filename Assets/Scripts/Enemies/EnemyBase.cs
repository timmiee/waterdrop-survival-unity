using UnityEngine;

namespace WaterDropSurvival.Enemies
{
    /// <summary>
    /// Base class for all enemy types in the game.
    /// Handles common enemy functionality like health, damage, and death.
    /// </summary>
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("Enemy Stats")]
        [SerializeField] protected string enemyName = "Enemy";
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth = 100f;
        [SerializeField] protected float moveSpeed = 2.5f; // 50% of base speed
        [SerializeField] protected float attackDamage = 33f;
        [SerializeField] protected float attackCooldown = 1f;
        
        [Header("Experience Drop")]
        [SerializeField] protected int experienceValue = 1;
        [SerializeField] protected GameObject experiencePrefab;
        
        [Header("Visual Effects")]
        [SerializeField] protected GameObject deathEffect;
        [SerializeField] protected Material hitFlashMaterial;
        [SerializeField] protected float hitFlashDuration = 0.1f;
        
        protected float nextAttackTime = 0f;
        protected Transform target;
        protected Rigidbody2D rb;
        protected SpriteRenderer spriteRenderer;
        protected Material originalMaterial;
        protected bool isDead = false;
        
        public bool IsDead => isDead;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (spriteRenderer != null)
            {
                originalMaterial = spriteRenderer.material;
            }
        }
        
        protected virtual void Start()
        {
            currentHealth = maxHealth;
            FindTarget();
        }
        
        protected virtual void Update()
        {
            if (isDead) return;
            
            if (target == null)
            {
                FindTarget();
            }
        }
        
        protected void FindTarget()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
        
        /// <summary>
        /// Apply damage to the enemy.
        /// </summary>
        public virtual void TakeDamage(float damage)
        {
            if (isDead) return;
            
            currentHealth -= damage;
            
            // Visual feedback
            OnHit();
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        protected virtual void OnHit()
        {
            // Flash effect
            if (spriteRenderer != null && hitFlashMaterial != null)
            {
                StartCoroutine(FlashRoutine());
            }
        }
        
        private System.Collections.IEnumerator FlashRoutine()
        {
            spriteRenderer.material = hitFlashMaterial;
            yield return new WaitForSeconds(hitFlashDuration);
            spriteRenderer.material = originalMaterial;
        }
        
        protected virtual void Die()
        {
            isDead = true;
            
            // Spawn experience orb
            if (experiencePrefab != null)
            {
                GameObject exp = Instantiate(experiencePrefab, transform.position, Quaternion.identity);
                var expPickup = exp.GetComponent<Systems.ExperiencePickup>();
                if (expPickup != null)
                {
                    expPickup.SetValue(experienceValue);
                }
            }
            
            // Spawn death effect
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
            
            // Destroy enemy
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Check if can attack the player.
        /// </summary>
        protected bool CanAttack()
        {
            return Time.time >= nextAttackTime;
        }
        
        /// <summary>
        /// Perform attack on player.
        /// </summary>
        protected void AttackPlayer()
        {
            if (!CanAttack()) return;
            
            var playerStats = target.GetComponent<Player.PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}
