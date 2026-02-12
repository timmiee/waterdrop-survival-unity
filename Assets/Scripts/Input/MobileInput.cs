using UnityEngine;

namespace WaterDropSurvival.Input
{
    /// <summary>
    /// Manages mobile touch input including virtual joystick and swipe detection.
    /// Provides centralized input handling for the player.
    /// </summary>
    public class MobileInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VirtualJoystick virtualJoystick;
        [SerializeField] private SwipeDetector swipeDetector;
        
        public Vector2 MoveInput => virtualJoystick != null ? virtualJoystick.InputDirection : Vector2.zero;
        
        private void Start()
        {
            // Auto-find components if not assigned
            if (virtualJoystick == null)
            {
                virtualJoystick = FindObjectOfType<VirtualJoystick>();
            }
            
            if (swipeDetector == null)
            {
                swipeDetector = FindObjectOfType<SwipeDetector>();
            }
        }
        
        private void Update()
        {
            // Fallback keyboard input for testing in editor
#if UNITY_EDITOR
            if (virtualJoystick == null)
            {
                // Use WASD or arrow keys
                float horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
                float vertical = UnityEngine.Input.GetAxisRaw("Vertical");
                // Store in a temporary variable if needed
            }
#endif
        }
    }
}
