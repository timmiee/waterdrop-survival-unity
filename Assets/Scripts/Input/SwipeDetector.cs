using UnityEngine;

namespace WaterDropSurvival.Input
{
    /// <summary>
    /// Detects swipe gestures on mobile devices for dash ability.
    /// Calculates swipe direction and triggers events.
    /// </summary>
    public class SwipeDetector : MonoBehaviour
    {
        [Header("Swipe Settings")]
        [SerializeField] private float minSwipeDistance = 50f;
        [SerializeField] private float maxSwipeTime = 0.5f;
        
        private Vector2 swipeStartPosition;
        private float swipeStartTime;
        private bool isSwiping = false;
        
        // Events
        public delegate void OnSwipeDetected(Vector2 direction);
        public event OnSwipeDetected OnSwipe;
        
        private void Update()
        {
            DetectSwipe();
        }
        
        private void DetectSwipe()
        {
            // Mobile touch input
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);
                
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        StartSwipe(touch.position);
                        break;
                        
                    case TouchPhase.Ended:
                        EndSwipe(touch.position);
                        break;
                }
            }
            // Mouse input for testing in editor
            else if (UnityEngine.Input.GetMouseButtonDown(1)) // Right mouse button
            {
                StartSwipe(UnityEngine.Input.mousePosition);
            }
            else if (UnityEngine.Input.GetMouseButtonUp(1))
            {
                EndSwipe(UnityEngine.Input.mousePosition);
            }
        }
        
        private void StartSwipe(Vector2 position)
        {
            isSwiping = true;
            swipeStartPosition = position;
            swipeStartTime = Time.time;
        }
        
        private void EndSwipe(Vector2 endPosition)
        {
            if (!isSwiping) return;
            
            isSwiping = false;
            
            float swipeTime = Time.time - swipeStartTime;
            float swipeDistance = Vector2.Distance(swipeStartPosition, endPosition);
            
            // Check if swipe is valid
            if (swipeTime <= maxSwipeTime && swipeDistance >= minSwipeDistance)
            {
                Vector2 swipeDirection = (endPosition - swipeStartPosition).normalized;
                OnSwipe?.Invoke(swipeDirection);
                
                Debug.Log($"Swipe detected: {swipeDirection}, Distance: {swipeDistance}, Time: {swipeTime}");
            }
        }
        
        /// <summary>
        /// Get the current swipe direction (useful for debugging).
        /// </summary>
        public Vector2 GetSwipeDirection()
        {
            if (isSwiping)
            {
                Vector2 currentPos = UnityEngine.Input.mousePosition;
                if (UnityEngine.Input.touchCount > 0)
                {
                    currentPos = UnityEngine.Input.GetTouch(0).position;
                }
                return (currentPos - swipeStartPosition).normalized;
            }
            return Vector2.zero;
        }
    }
}
