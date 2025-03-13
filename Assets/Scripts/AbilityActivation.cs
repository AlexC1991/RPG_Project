using UnityEngine;

namespace RPGGame
{
    public class AbilityActivation : MonoBehaviour
    {
        [SerializeField] private GameObject[] abilityPrefabs;
        [SerializeField] private Transform shootingPoint;
        private PlayerPowers playerP;
        
        private void Awake()
        {
            playerP = FindObjectOfType<PlayerPowers>();
        }

        public void ActivateTheAbility(string idAbility)
        {
            foreach (var power in playerP.playersPowers)
            {
                if (power.id == idAbility)
                {
                    GameObject ability = Instantiate(abilityPrefabs[0], shootingPoint.position, Quaternion.identity);
                    ability.GetComponent<Rigidbody>().AddForce(transform.forward * power.range, ForceMode.Impulse);
                }
            }
        }

    }
}
