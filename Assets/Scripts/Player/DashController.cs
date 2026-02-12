using UnityEngine;
using WaterDropSurvival.Input;

namespace WaterDropSurvival.Player
{
    /// <summary>
    /// Handles player dash ability triggered by swipe gestures.
    /// Provides invulnerability frames and speed boost during dash.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStats))]
    public class DashController : MonoBehaviour
    {
        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed = 15f;
        [SerializeField] private float dashDuration = 0.3f;
        [SerializeField] private float dashCooldown = 1.5f;
        
        [Header("Dash Effects")]
        [SerializeField] private bool invulnerableDuringDash = true;
        [SerializeField] private TrailRenderer dashTrail;
        [SerializeField] private GameObject dashParticles;
        
        [Header("References")]
        [SerializeField] private SwipeDetector swipeDetector;
        
        private Rigidbody2D rb;
        private PlayerStats stats;
        private PlayerPhysics playerPhysics;
        private PlayerController playerController;
        
        private bool isDashing = false;
        private bool canDash = true;
        private float dashTimer = 0f;
        private float cooldownTimer = 0f;
        private Vector2 dashDirection;
        
        // Layer mask for dash collision
        private int originalLayer;
        
        public bool IsDashing => isDashing;
        public float DashCooldownPercent => 1f - (cooldownTimer / dashCooldown);
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            stats = GetComponent<PlayerStats>();
            playerPhysics = GetComponent<PlayerPhysics>();
            playerController = GetComponent<PlayerController>();
            
            originalLayer = gameObject.layer;
        }
        
        private void Start()
        {
            // Find swipe detector if not assigned
            if (swipeDetector == null)
            {
                swipeDetector = FindObjectOfType<SwipeDetector>();
            }
            
            // Setup trail renderer
            if (dashTrail != null)
            {
                dashTrail.emitting = false;
            }
        }
        
        private void OnEnable()
        {
            if (swipeDetector != null)
            {
                swipeDetector.OnSwipe += HandleSwipe;
            }
        }
        
        private void OnDisable()
        {
            if (swipeDetector != null)
            {
                swipeDetector.OnSwipe -= HandleSwipe;
            }
        }
        
        private void Update()
        {
            if (!stats.IsAlive) return;
            
            // Update cooldown
            if (!canDash)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0f)
                {
                    canDash = true;
                    cooldownTimer = 0f;
                }
            }
            
            // Update dash duration
            if (isDashing)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f)
                {
                    EndDash();
                }
            }
        }
        
        private void FixedUpdate()
        {
            if (isDashing)
            {
                // Apply dash velocity
                rb.velocity = dashDirection * dashSpeed;
            }
        }
        
        private void HandleSwipe(Vector2 swipeDirection)
        {
            if (canDash && !isDashing && stats.IsAlive)
            {
                StartDash(swipeDirection);
            }
        }
        
        /// <summary>
        /// Initiate a dash in the specified direction.
        /// </summary>
        public void StartDash(Vector2 direction)
        {
            if (!canDash || isDashing) return;
            
            isDashing = true;
            canDash = false;
            dashTimer = dashDuration;
            cooldownTimer = dashCooldown;
            dashDirection = direction.normalized;
            
            // Apply visual effects
            if (dashTrail != null)
            {
                dashTrail.emitting = true;
            }
            
            if (dashParticles != null)
            {
                Instantiate(dashParticles, transform.position, Quaternion.identity);
            }
            
            // Apply squish effect
            if (playerPhysics != null)
            {
                playerPhysics.ApplyDashSquish(dashDirection);
            }
            
            // Make invulnerable during dash
            if (invulnerableDuringDash)
            {
                // Change to a layer that doesn't collide with enemies
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            
            Debug.Log($"Dash started in direction: {dashDirection}");
        }
        
        private void EndDash()
        {
            isDashing = false;
            
            // Restore normal velocity
            rb.velocity = Vector2.zero;
            
            // Disable visual effects
            if (dashTrail != null)
            {
                dashTrail.emitting = false;
            }
            
            // Restore collision layer
            if (invulnerableDuringDash)
            {
                gameObject.layer = originalLayer;
            }
        }
        
        /// <summary>
        /// Public method to trigger dash (for testing or other systems).
        /// </summary>
        public void TriggerDash(Vector2 direction)
        {
            StartDash(direction);
        }
    }
}
