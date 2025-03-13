using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame
{
    public class PlayerPowers : MonoBehaviour
    {
        [SerializeField] private PowerDataBase playerPowersToLearn;
        public List<PowerSelectionScript.PowerSelection> playersPowers = new List<PowerSelectionScript.PowerSelection>();
        [TagSelector] [SerializeField] private string powerBox;
        [SerializeField] private Image displayedIcon;
        private void Start()
        {
            playerPowersToLearn = FindObjectOfType<PowerDataBase>();
            playersPowers.Clear();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(powerBox))
            {
                var power = playerPowersToLearn.FindPower("1F");
        
                // Only proceed if a valid power was found
                if (power.id != null)
                {
                    if (!PowerExists(power.id))
                    {
                        AddToMyPowers(power);
                    }
                    else
                    {
                        Debug.Log($"Already know power: {power.name}");
                    }
                }
            }
        }
        
        private bool PowerExists(string id)
        {
            foreach (var power in playersPowers)
            {
                if (power.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddToMyPowers(PowerSelectionScript.PowerSelection power)
        {
                playersPowers.Add(power);
                Debug.Log($"Learned new power: {power.name}");
                displayedIcon.sprite = power.icon;
                
    
                
                
           // Debugging Code     
            /*Debug.Log($"Total powers: {playersPowers.Count}");

            foreach (var g in playersPowers)
            {
                Debug.Log($"Power: {g.name}");
            }*/
        }
    }
}
