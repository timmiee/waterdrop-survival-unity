using UnityEngine;

namespace WaterDropSurvival
{
    /// <summary>
    /// Camera controller that follows the player in portrait mode.
    /// Provides smooth camera movement and boundary constraints.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private bool findPlayerOnStart = true;
        
        [Header("Follow Settings")]
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
        [SerializeField] private bool smoothFollow = true;
        
        [Header("Camera Bounds")]
        [SerializeField] private bool useBounds = true;
        [SerializeField] private float minX = -50f;
        [SerializeField] private float maxX = 50f;
        [SerializeField] private float minY = -50f;
        [SerializeField] private float maxY = 50f;
        
        [Header("Portrait Mode")]
        [SerializeField] private float portraitOrthographicSize = 6f;
        
        private Camera cam;
        
        private void Awake()
        {
            cam = GetComponent<Camera>();
            
            // Set camera for portrait mode
            if (cam != null)
            {
                cam.orthographicSize = portraitOrthographicSize;
            }
        }
        
        private void Start()
        {
            if (findPlayerOnStart && target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
            }
        }
        
        private void LateUpdate()
        {
            if (target == null) return;
            
            FollowTarget();
        }
        
        private void FollowTarget()
        {
            Vector3 targetPosition = target.position + offset;
            
            // Apply bounds if enabled
            if (useBounds)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            }
            
            // Smooth or instant follow
            if (smoothFollow)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    smoothSpeed * Time.deltaTime
                );
            }
            else
            {
                transform.position = targetPosition;
            }
        }
        
        /// <summary>
        /// Set the camera target manually.
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
        
        /// <summary>
        /// Set camera bounds.
        /// </summary>
        public void SetBounds(float minXBound, float maxXBound, float minYBound, float maxYBound)
        {
            minX = minXBound;
            maxX = maxXBound;
            minY = minYBound;
            maxY = maxYBound;
            useBounds = true;
        }
        
        // Visualize camera bounds in editor
        private void OnDrawGizmosSelected()
        {
            if (useBounds)
            {
                Gizmos.color = Color.yellow;
                Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0);
                Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
