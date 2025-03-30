using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        public float _currentHealth;
        private float _maxHealth = 100f;
        private float _healthRegenRate;
        private float _healthRegenDelay = 1f;
        private float _minHealth = 0f;
        private float _healthRegenTimer;
        private float duration;
        private Coroutine _healthRegen;
        
        
        private void Start()
        {
            _currentHealth = _maxHealth;
            _healthBar.maxValue = _maxHealth;
            _healthBar.value = _currentHealth;
        }
        
        public void StartHealthRegen(float healthRegenAmount)
        {
            _healthRegen = StartCoroutine(IncreaseHealth(healthRegenAmount));
        }

        public void StopHealthRegen()
        {
            StopCoroutine(_healthRegen);
        }

        public void HealthIncrease(float healthAmount)
        {
            _currentHealth += healthAmount;
            _healthBar.value = _currentHealth;
            ShowHealthIncreaseText(healthAmount);
        }

        public void UpdateHealthBar()
        {
            _healthBar.value = _currentHealth;
        }
        
        private void ShowHealthIncreaseText(float healthAmount)
        {
            // Create a new GameObject for the text
            GameObject healthTextGO = new GameObject("HealthIncreaseText");
            healthTextGO.transform.position = transform.position + new Vector3(0, 2, 0); // Position text above the player

            // Add a Text component to the GameObject (or TMP_Text for TextMeshPro)
            TextMeshPro textMesh = healthTextGO.AddComponent<TextMeshPro>();
    
            // Configure the text component
            textMesh.text = $"+{healthAmount}";
            textMesh.fontSize = 5;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.color = Color.green;

            // Optionally: Add animation to make the text 'float' upwards
            StartCoroutine(AnimateAndDestroyText(healthTextGO));
        }

        private IEnumerator AnimateAndDestroyText(GameObject healthTextGO)
        {
            float duration = 1f; // Text lifetime in seconds
            Vector3 initialPosition = healthTextGO.transform.position;
            Vector3 targetPosition = initialPosition + new Vector3(0, 1, 0); // Float up

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                // Lerp the text position upwards over time
                healthTextGO.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Destroy the text GameObject after the animation
            Destroy(healthTextGO);
        }


        private IEnumerator IncreaseHealth(float healthRegenAmount)
        {
            while (true)
            {
                yield return new WaitForSeconds(_healthRegenDelay);
                if (_currentHealth < _maxHealth)
                {
                    _currentHealth += healthRegenAmount;
                    _healthBar.value = _currentHealth;
                    duration -= 1f;
                }

                yield return null;
            }
        }
    }
}
