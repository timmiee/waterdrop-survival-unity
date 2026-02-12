using UnityEngine;

namespace WaterDropSurvival.Enemies
{
    /// <summary>
    /// AI controller for enemies that follows and attacks the player.
    /// Implements simple chase behavior with collision avoidance.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyAI : MonoBehaviour
    {
        [Header("AI Settings")]
        [SerializeField] private float detectionRange = 15f;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float avoidanceRadius = 0.5f;
        
        [Header("References")]
        private EnemyBase enemyBase;
        private Rigidbody2D rb;
        private Transform target;
        
        private void Awake()
        {
            enemyBase = GetComponent<EnemyBase>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            // Find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
        
        private void FixedUpdate()
        {
            if (enemyBase.IsDead || target == null) return;
            
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            
            // Check if player is in detection range
            if (distanceToTarget <= detectionRange)
            {
                if (distanceToTarget > attackRange)
                {
                    // Move towards player
                    MoveTowardsTarget();
                }
                else
                {
                    // Stop and attack
                    rb.velocity = Vector2.zero;
                }
            }
            else
            {
                // Stop moving if player is out of range
                rb.velocity = Vector2.zero;
            }
        }
        
        private void MoveTowardsTarget()
        {
            Vector2 direction = (target.position - transform.position).normalized;
            
            // Apply simple avoidance
            direction = ApplyAvoidance(direction);
            
            // Move enemy
            rb.velocity = direction * GetMoveSpeed();
        }
        
        private Vector2 ApplyAvoidance(Vector2 currentDirection)
        {
            // Simple avoidance from nearby enemies
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);
            Vector2 avoidanceVector = Vector2.zero;
            
            foreach (Collider2D col in nearbyEnemies)
            {
                if (col.gameObject != gameObject && col.CompareTag("Enemy"))
                {
                    Vector2 awayFromEnemy = (transform.position - col.transform.position).normalized;
                    avoidanceVector += awayFromEnemy;
                }
            }
            
            // Blend avoidance with target direction
            Vector2 finalDirection = (currentDirection + avoidanceVector * 0.3f).normalized;
            return finalDirection;
        }
        
        private float GetMoveSpeed()
        {
            // Get move speed from EnemyBase
            return enemyBase != null ? 2.5f : 2f;
        }
        
        private void OnCollisionStay2D(Collision2D collision)
        {
            // Attack player on collision
            if (collision.gameObject.CompareTag("Player"))
            {
                var playerStats = collision.gameObject.GetComponent<Player.PlayerStats>();
                if (playerStats != null && Time.time >= Time.time + 1f)
                {
                    playerStats.TakeDamage(33f); // Default damage
                }
            }
        }
        
        // Visualize detection range in editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
