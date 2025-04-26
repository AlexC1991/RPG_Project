using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    public class AbilityActivation : MonoBehaviour
    {
        [SerializeField] private Transform shootingPoint;
        private PlayerHealthController playerHC;
        private PlayerPowers playerP;
        private HashSet<string> abilitiesOnCooldown = new HashSet<string>();
        private Coroutine aoeEffect;
        private float aoeDuratio;
        [SerializeField] private Animator playerAnimaton;

        private void Awake()
        {
            playerHC = GetComponent<PlayerHealthController>();
            playerP = FindObjectOfType<PlayerPowers>();
        }

        public void ActivateTheAbility(string idAbility)
        {
            // Check if the ability is on cooldown
            if (abilitiesOnCooldown.Contains(idAbility))
            {
                Debug.Log($"Ability {idAbility} is still on cooldown!");
                return; // Exit if the ability is still on cooldown
            }

            // Find the ability in player powers
            foreach (var power in playerP.playersPowers)
            {
                if (power.id == idAbility)
                {
                    // Start a coroutine that handles animation and delayed ability activation
                    StartCoroutine(PlayAnimationAndActivateAbility(power, idAbility));
                    return; // Exit after finding and using the ability

                }
            }
        }

        private IEnumerator PlayAnimationAndActivateAbility(PowerSelectionScript.PowerSelection power, string idAbility)
        {
            // Store the original rotation before playing animation
            Quaternion originalRotation = Camera.main != null ? Camera.main.transform.rotation : transform.rotation;

            // Play the animation if it exists
            if (power.abilityAnimationClip != null && playerAnimaton != null)
            {
                // Set animator to play animation without affecting rotation
                playerAnimaton.applyRootMotion = false;
                playerAnimaton.Play(power.abilityAnimationClip.name);
                Debug.Log($"Playing animation: {power.abilityAnimationClip.name}");

                // Calculate delay time based on animation length
                float delayTime = power.abilityAnimationClip.length * 0.5f;

                // Wait for the animation to play partially before activating the ability
                yield return new WaitForSeconds(delayTime);
            }

            // Activate the ability based on its type
            switch (power.powerType)
            {
                case (PowerSelectionScript.PowerType.Projectile):
                    // Get the camera's forward direction for aiming
                    Vector3 shootDirection = Camera.main != null ? Camera.main.transform.forward : transform.forward;

                    // Use the original shooting direction
                    Quaternion projectileRotation = Quaternion.LookRotation(shootDirection);

                    // Instantiate the projectile with the correct rotation
                    GameObject ability = Instantiate(power.abilityPrefabs, shootingPoint.position, projectileRotation);

                    // Add force in the shooting direction
                    ability.GetComponent<Rigidbody>()
                        .AddForce(shootDirection * power.projectileType.speed, ForceMode.Impulse);

                    // Set the destruction timer
                    Destroy(ability, power.projectileType.range);

                    StartCoroutine(StartAbilityCooldown(idAbility, power.coolDownTimer));
                    break;

                // Other cases remain unchanged
                case (PowerSelectionScript.PowerType.Buff):
                    ApplyBuffToPlayer(power);
                    StartCoroutine(StartAbilityCooldown(idAbility, power.coolDownTimer));
                    break;

                case (PowerSelectionScript.PowerType.AOE):
                    ApplyAOEEffect(power);
                    StartCoroutine(StartAbilityCooldown(idAbility, power.coolDownTimer));
                    break;

                default:
                    Debug.LogWarning($"Unknown ability type: {power.powerType}");
                    break;
            }
        }

        // Coroutine to handle ability cooldown
        private IEnumerator StartAbilityCooldown(string abilityId, float cooldownTime)
        {
            // Mark ability as on cooldown
            abilitiesOnCooldown.Add(abilityId);

            float remainingTime = cooldownTime;

            while (remainingTime > 0)
            {
                yield return null; // Wait for next frame
                remainingTime -= Time.deltaTime;
            }

            // Cooldown complete
            abilitiesOnCooldown.Remove(abilityId);
            Debug.Log($"Ability {abilityId} is ready to use again!");
        }
        
        private void ApplyBuffToPlayer(PowerSelectionScript.PowerSelection power)
        {
            switch (power.buffType.buffAbility)
            {
                case PowerSelectionScript.BuffAbility.Health:
                    // Increase player health
                    GameObject healthHeal = Instantiate(power.abilityPrefabs, transform.position, Quaternion.identity);
                    playerHC.HealthIncrease(power.buffType.buffValue);
                    Destroy(healthHeal.gameObject, power.buffType.duration);
                    break;

                /*case PowerSelectionScript.BuffAbility.Damage:
                    // Temporarily increase player damage
                    playerP.ModifyDamage(power.buffType.buffValue, power.buffType.duration);
                    break;*/

                /*case PowerSelectionScript.BuffAbility.Speed:
                    // Temporarily increase player movement speed
                    playerP.ModifySpeed(power.buffType.buffValue, power.buffType.duration);
                    break;*/

                default:
                    Debug.LogWarning("Unknown Buff Ability!");
                    break;
            }
        }
        
        private void ApplyAOEEffect(PowerSelectionScript.PowerSelection power)
        {
            {
                Vector3 locationToSpawn = new Vector3(transform.position.x, 0, transform.position.z);
                GameObject aoePrefab = Instantiate(power.abilityPrefabs, locationToSpawn, Quaternion.identity);
                aoePrefab.transform.localScale = new Vector3(1,0.3f,1);
                aoeEffect = StartCoroutine(AOERadiusIncrease(aoePrefab, power));
            }
        }

        private IEnumerator AOERadiusIncrease(GameObject aoeP, PowerSelectionScript.PowerSelection power)
        {
            // Set the desired scale for the AOE radius
            Vector3 targetScale = new Vector3(power.aeoType.radius, 0.4f, power.aeoType.radius); 
            float duration = 5f; // Example duration for the AOE effect
            float elapsedTime = 0f;

            // Gradually scale the AOE prefab over the duration
            Vector3 initialScale = aoeP.transform.localScale;
            while (elapsedTime < duration)
            {
                aoeP.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
                elapsedTime += 8f * Time.deltaTime;
                yield return null;
            }

            // Ensure the final scale is exactly the targetScale after the loop
            aoeP.transform.localScale = targetScale;

            // Activate the particle effect on child 0 of the prefab
            if (aoeP.transform.childCount > 0)
            {
                Transform child = aoeP.transform.GetChild(0);
                ParticleSystem particle = child.GetComponent<ParticleSystem>();

                if (particle != null)
                {
                    particle.Play(); // Start the particle effect
                    playerHC.StartHealthRegen(power.aeoType.abilityValue);
                }
                else
                {
                    Debug.LogWarning("Child does not have a ParticleSystem component!");
                }
            }
            else
            {
                Debug.LogWarning("Prefab has no children to activate particle effect!");
            }

            // Wait for the duration of the AOE effect
            yield return new WaitForSeconds(power.aeoType.duration);
            playerHC.StopHealthRegen();
            Destroy(aoeP.gameObject);

        }
    }
}

