using UnityEngine;
using WaterDropSurvival.Input;

namespace WaterDropSurvival.Player
{
    /// <summary>
    /// Controls player movement with mobile joystick input and smooth physics.
    /// Includes inertia for squishy water drop feel.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float deceleration = 15f;
        
        [Header("References")]
        [SerializeField] private MobileInput mobileInput;
        
        private Rigidbody2D rb;
        private PlayerStats stats;
        private Vector2 currentVelocity;
        private Vector2 moveInput;
        
        public Vector2 LastMoveDirection { get; private set; }
        public bool IsMoving => moveInput.magnitude > 0.1f;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            stats = GetComponent<PlayerStats>();
            
            // Configure rigidbody for smooth physics
            rb.gravityScale = 0f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
        private void Start()
        {
            // Find mobile input if not assigned
            if (mobileInput == null)
            {
                mobileInput = FindObjectOfType<MobileInput>();
            }
        }
        
        private void Update()
        {
            // Get input from mobile joystick
            if (mobileInput != null)
            {
                moveInput = mobileInput.MoveInput;
            }
            else
            {
                // Fallback to keyboard for testing
                moveInput = new Vector2(
                    UnityEngine.Input.GetAxisRaw("Horizontal"),
                    UnityEngine.Input.GetAxisRaw("Vertical")
                ).normalized;
            }
            
            // Track last move direction for weapon aiming
            if (moveInput.magnitude > 0.1f)
            {
                LastMoveDirection = moveInput.normalized;
            }
        }
        
        private void FixedUpdate()
        {
            if (!stats.IsAlive) return;
            
            MovePlayer();
        }
        
        private void MovePlayer()
        {
            float moveSpeed = stats.MoveSpeed;
            Vector2 targetVelocity = moveInput.normalized * moveSpeed;
            
            // Smooth acceleration/deceleration for inertia feel
            if (moveInput.magnitude > 0.1f)
            {
                currentVelocity = Vector2.MoveTowards(
                    currentVelocity,
                    targetVelocity,
                    acceleration * Time.fixedDeltaTime
                );
            }
            else
            {
                currentVelocity = Vector2.MoveTowards(
                    currentVelocity,
                    Vector2.zero,
                    deceleration * Time.fixedDeltaTime
                );
            }
            
            rb.velocity = currentVelocity;
        }
        
        /// <summary>
        /// Apply knockback to the player (e.g., when hit by enemy).
        /// </summary>
        public void ApplyKnockback(Vector2 direction, float force)
        {
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }
}
