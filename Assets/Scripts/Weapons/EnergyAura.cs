using UnityEngine;
using WaterDropSurvival.Player;
using System.Collections.Generic;

namespace WaterDropSurvival.Weapons
{
    /// <summary>
    /// Energy Aura weapon - Unlocked at level 10.
    /// Creates a rotating aura that damages enemies on contact.
    /// </summary>
    public class EnergyAura : WeaponBase
    {
        [Header("Aura Settings")]
        [SerializeField] private float auraRadius = 3f;
        [SerializeField] private float rotationSpeed = 180f; // Degrees per second
        [SerializeField] private int numberOfOrbs = 3;
        
        [Header("Orb Prefab")]
        [SerializeField] private GameObject orbPrefab;
        
        private List<GameObject> orbs = new List<GameObject>();
        private PlayerStats playerStats;
        private float currentRotation = 0f;
        
        protected override void Start()
        {
            base.Start();
            weaponName = "Energy Aura";
            baseDamage = 15f;
            fireRate = 2f; // Damage tick rate
            
            playerStats = GetComponentInParent<PlayerStats>();
            
            SpawnOrbs();
        }
        
        protected override void Update()
        {
            // Rotate orbs around player
            currentRotation += rotationSpeed * Time.deltaTime;
            if (currentRotation >= 360f)
                currentRotation -= 360f;
            
            UpdateOrbPositions();
            
            // Call base update for damage ticks
            base.Update();
        }
        
        protected override void Fire()
        {
            // Damage enemies within aura radius
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, auraRadius);
            
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    var enemy = hit.GetComponent<WaterDropSurvival.Enemies.EnemyBase>();
                    if (enemy != null && playerStats != null)
                    {
                        enemy.TakeDamage(playerStats.CalculateDamage(baseDamage));
                    }
                }
            }
        }
        
        private void SpawnOrbs()
        {
            if (orbPrefab == null) return;
            
            for (int i = 0; i < numberOfOrbs; i++)
            {
                GameObject orb = Instantiate(orbPrefab, transform);
                orbs.Add(orb);
            }
        }
        
        private void UpdateOrbPositions()
        {
            float angleStep = 360f / numberOfOrbs;
            
            for (int i = 0; i < orbs.Count; i++)
            {
                if (orbs[i] == null) continue;
                
                float angle = (currentRotation + (angleStep * i)) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(
                    Mathf.Cos(angle) * auraRadius,
                    Mathf.Sin(angle) * auraRadius,
                    0
                );
                
                orbs[i].transform.position = transform.position + offset;
            }
        }
        
        public override void Upgrade()
        {
            base.Upgrade();
            
            // Add more orbs or increase radius on upgrade
            if (weaponLevel % 2 == 0 && numberOfOrbs < 6)
            {
                numberOfOrbs++;
                GameObject orb = Instantiate(orbPrefab, transform);
                orbs.Add(orb);
            }
            else
            {
                auraRadius += 0.5f;
            }
        }
        
        private void OnDestroy()
        {
            // Clean up orbs
            foreach (GameObject orb in orbs)
            {
                if (orb != null)
                    Destroy(orb);
            }
        }
        
        // Visualize aura range in editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, auraRadius);
        }
    }
}
