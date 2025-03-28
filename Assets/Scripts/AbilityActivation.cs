using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame
{
    public class AbilityActivation : MonoBehaviour
    {
        [SerializeField] private Transform shootingPoint;
        private PlayerPowers playerP;
        private HashSet<string> abilitiesOnCooldown = new HashSet<string>();

        private void Awake()
        {
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
                    // Instantiate and activate the ability
                    GameObject ability = Instantiate(power.abilityPrefabs, shootingPoint.position, Quaternion.identity);
                    ability.GetComponent<Rigidbody>().AddForce(transform.forward * power.range, ForceMode.Impulse);
                    Destroy(ability, power.range);

                    // Start cooldown coroutine for this ability
                    StartCoroutine(StartAbilityCooldown(idAbility, power.coolDownTimer));

                    Debug.Log($"Ability {idAbility} used! Cooldown started: {power.coolDownTimer} seconds.");
                    break;
                }
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

                // Optional: You could have an event here to update UI with remaining cooldown
                // OnCooldownUpdated?.Invoke(abilityId, remainingTime);
            }

            // Cooldown complete
            abilitiesOnCooldown.Remove(abilityId);
            Debug.Log($"Ability {abilityId} is ready to use again!");
        }
    }
}

