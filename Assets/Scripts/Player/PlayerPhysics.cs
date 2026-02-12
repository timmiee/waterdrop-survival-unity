using UnityEngine;

namespace WaterDropSurvival.Player
{
    /// <summary>
    /// Manages squishy water drop physics including visual deformation and particle effects.
    /// Creates the soft-body jelly physics effect for the water drop character.
    /// </summary>
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerPhysics : MonoBehaviour
    {
        [Header("Squishy Physics")]
        [SerializeField] private float squishSpeed = 10f;
        [SerializeField] private float squishAmount = 0.2f;
        [SerializeField] private float bounceBackSpeed = 8f;
        
        [Header("Particle Effects")]
        [SerializeField] private GameObject dropletParticlePrefab;
        [SerializeField] private int dropletsOnHit = 5;
        [SerializeField] private float dropletLifetime = 1f;
        
        [Header("Visual References")]
        [SerializeField] private Transform bodyTransform;
        
        private Vector3 originalScale;
        private Vector3 targetScale;
        private PlayerStats stats;
        private Rigidbody2D rb;
        
        // Squish state
        private bool isSquishing = false;
        private Vector2 squishDirection;
        
        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
            rb = GetComponent<Rigidbody2D>();
            
            if (bodyTransform == null)
            {
                bodyTransform = transform;
            }
            
            originalScale = bodyTransform.localScale;
            targetScale = originalScale;
        }
        
        private void OnEnable()
        {
            stats.HealthChanged += OnHealthChanged;
        }
        
        private void OnDisable()
        {
            stats.HealthChanged -= OnHealthChanged;
        }
        
        private void Update()
        {
            UpdateSquishEffect();
        }
        
        private void FixedUpdate()
        {
            // Apply movement-based squish
            if (rb.velocity.magnitude > 0.5f)
            {
                ApplyMovementSquish(rb.velocity.normalized);
            }
        }
        
        private void UpdateSquishEffect()
        {
            // Smoothly interpolate back to original scale
            bodyTransform.localScale = Vector3.Lerp(
                bodyTransform.localScale,
                targetScale,
                Time.deltaTime * bounceBackSpeed
            );
            
            // Reset target scale when close enough
            if (Vector3.Distance(bodyTransform.localScale, originalScale) < 0.01f)
            {
                targetScale = originalScale;
                isSquishing = false;
            }
        }
        
        /// <summary>
        /// Apply squish effect based on movement direction.
        /// </summary>
        private void ApplyMovementSquish(Vector2 direction)
        {
            if (isSquishing) return;
            
            float velocityFactor = Mathf.Clamp01(rb.velocity.magnitude / 10f);
            float squish = squishAmount * velocityFactor;
            
            // Squish perpendicular to movement direction
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Moving horizontally
                targetScale = new Vector3(
                    originalScale.x * (1f - squish),
                    originalScale.y * (1f + squish),
                    originalScale.z
                );
            }
            else
            {
                // Moving vertically
                targetScale = new Vector3(
                    originalScale.x * (1f + squish),
                    originalScale.y * (1f - squish),
                    originalScale.z
                );
            }
        }
        
        /// <summary>
        /// Apply impact squish when player takes damage.
        /// </summary>
        public void ApplyImpactSquish(Vector2 impactDirection)
        {
            isSquishing = true;
            squishDirection = impactDirection.normalized;
            
            // Squish in the direction of impact
            float angle = Mathf.Atan2(impactDirection.y, impactDirection.x) * Mathf.Rad2Deg;
            float squishX = 1f - squishAmount * Mathf.Abs(Mathf.Cos(angle * Mathf.Deg2Rad));
            float squishY = 1f - squishAmount * Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad));
            
            targetScale = new Vector3(
                originalScale.x * squishX,
                originalScale.y * squishY,
                originalScale.z
            );
            
            // Spawn droplet particles
            SpawnDropletParticles(impactDirection);
        }
        
        /// <summary>
        /// Spawn water droplet particles on hit.
        /// </summary>
        private void SpawnDropletParticles(Vector2 direction)
        {
            if (dropletParticlePrefab == null) return;
            
            for (int i = 0; i < dropletsOnHit; i++)
            {
                // Random spread around impact direction
                float angle = Mathf.Atan2(direction.y, direction.x) + Random.Range(-45f, 45f) * Mathf.Deg2Rad;
                Vector2 particleDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                
                GameObject droplet = Instantiate(dropletParticlePrefab, transform.position, Quaternion.identity);
                
                // Apply physics to droplet
                Rigidbody2D dropletRb = droplet.GetComponent<Rigidbody2D>();
                if (dropletRb != null)
                {
                    dropletRb.velocity = particleDirection * Random.Range(2f, 5f);
                }
                
                // Destroy after lifetime
                Destroy(droplet, dropletLifetime);
            }
        }
        
        private void OnHealthChanged(float current, float max)
        {
            // Scale down the player slightly as health decreases
            float healthPercent = current / max;
            originalScale = Vector3.one * Mathf.Lerp(0.8f, 1f, healthPercent);
        }
        
        /// <summary>
        /// Trigger dash squish effect.
        /// </summary>
        public void ApplyDashSquish(Vector2 dashDirection)
        {
            isSquishing = true;
            
            // Extreme squish in dash direction
            float angle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;
            targetScale = new Vector3(
                originalScale.x * 0.6f,
                originalScale.y * 1.4f,
                originalScale.z
            );
        }
    }
}
