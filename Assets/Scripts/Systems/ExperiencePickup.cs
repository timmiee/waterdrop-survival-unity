using UnityEngine;

namespace WaterDropSurvival.Systems
{
    /// <summary>
    /// Experience pickup that the player can collect.
    /// Spawned when enemies die.
    /// </summary>
    public class ExperiencePickup : MonoBehaviour
    {
        [Header("Experience Settings")]
        [SerializeField] private int experienceValue = 1;
        [SerializeField] private float attractionRange = 3f;
        [SerializeField] private float attractionSpeed = 5f;
        [SerializeField] private float pickupRange = 0.5f;
        
        [Header("Visual")]
        [SerializeField] private float rotationSpeed = 180f;
        
        private Transform playerTransform;
        private bool isBeingAttracted = false;
        
        public void SetValue(int value)
        {
            experienceValue = value;
        }
        
        private void Start()
        {
            // Find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        private void Update()
        {
            if (playerTransform == null) return;
            
            // Rotate for visual effect
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            
            // Start attraction when player is close
            if (distanceToPlayer <= attractionRange)
            {
                isBeingAttracted = true;
            }
            
            // Move towards player
            if (isBeingAttracted)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                transform.position += (Vector3)direction * attractionSpeed * Time.deltaTime;
                
                // Check if close enough to pick up
                if (distanceToPlayer <= pickupRange)
                {
                    PickUp();
                }
            }
        }
        
        private void PickUp()
        {
            // Give experience to player
            var experienceManager = FindObjectOfType<ExperienceManager>();
            if (experienceManager != null)
            {
                experienceManager.AddExperience(experienceValue);
            }
            
            // Destroy pickup
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PickUp();
            }
        }
    }
}
