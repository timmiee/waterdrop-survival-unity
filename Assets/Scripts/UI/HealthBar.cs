using UnityEngine;
using UnityEngine.UI;
using WaterDropSurvival.Player;

namespace WaterDropSurvival.UI
{
    /// <summary>
    /// Displays player health bar and updates visually.
    /// Supports smooth transitions and color changes based on health percentage.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image fillImage;
        [SerializeField] private Image backgroundImage;
        
        [Header("Settings")]
        [SerializeField] private bool smoothTransition = true;
        [SerializeField] private float transitionSpeed = 5f;
        
        [Header("Color Gradient")]
        [SerializeField] private Gradient healthColorGradient;
        [SerializeField] private bool useDefaultGradient = true;
        
        [Header("Player Reference")]
        [SerializeField] private PlayerStats playerStats;
        
        private float targetFillAmount = 1f;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindObjectOfType<PlayerStats>();
            }
            
            // Subscribe to health changes
            if (playerStats != null)
            {
                playerStats.HealthChanged += OnHealthChanged;
                UpdateHealthBar(playerStats.CurrentHealth, playerStats.MaxHealth);
            }
            
            // Setup default gradient if needed
            if (useDefaultGradient)
            {
                SetupDefaultGradient();
            }
        }
        
        private void OnDestroy()
        {
            if (playerStats != null)
            {
                playerStats.HealthChanged -= OnHealthChanged;
            }
        }
        
        private void Update()
        {
            if (smoothTransition && fillImage != null)
            {
                // Smoothly interpolate to target fill amount
                fillImage.fillAmount = Mathf.Lerp(
                    fillImage.fillAmount,
                    targetFillAmount,
                    Time.deltaTime * transitionSpeed
                );
            }
        }
        
        private void OnHealthChanged(float current, float max)
        {
            UpdateHealthBar(current, max);
        }
        
        private void UpdateHealthBar(float current, float max)
        {
            if (fillImage == null) return;
            
            float healthPercent = Mathf.Clamp01(current / max);
            targetFillAmount = healthPercent;
            
            if (!smoothTransition)
            {
                fillImage.fillAmount = targetFillAmount;
            }
            
            // Update color based on health percentage
            if (healthColorGradient != null)
            {
                fillImage.color = healthColorGradient.Evaluate(healthPercent);
            }
        }
        
        private void SetupDefaultGradient()
        {
            healthColorGradient = new Gradient();
            
            GradientColorKey[] colorKeys = new GradientColorKey[3];
            colorKeys[0] = new GradientColorKey(Color.red, 0f);      // 0% health
            colorKeys[1] = new GradientColorKey(Color.yellow, 0.5f); // 50% health
            colorKeys[2] = new GradientColorKey(Color.green, 1f);    // 100% health
            
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1f, 0f);
            alphaKeys[1] = new GradientAlphaKey(1f, 1f);
            
            healthColorGradient.SetKeys(colorKeys, alphaKeys);
        }
    }
}
