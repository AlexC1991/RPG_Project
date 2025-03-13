using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGGame
{
    public class PowerDataBase : MonoBehaviour
    { 
        private PowerSelectionScript powers;
        private AbilityIconDataBase abilityIconData;
        public HashSet<PowerSelectionScript.PowerSelection> powerData = new HashSet<PowerSelectionScript.PowerSelection>();
        private void Start()
        {
            powers = FindObjectOfType<PowerSelectionScript>();
            abilityIconData = FindObjectOfType<AbilityIconDataBase>();
            ListOfPowers();
        }
        
        public void ListOfPowers()
        {
            powerData.Clear();
            
            AddPower(new PowerSelectionScript.PowerSelection
            {
                level = 1,
                id = "1F",
                name = "Fire Ball",
                power = PowerSelectionScript.Powers.Fire,
                damage = 2,
                range = 5,
                cooldown = 5,
                icon = abilityIconData.icons[0]
            });
            
            AddPower(new PowerSelectionScript.PowerSelection
            {
                level = 1,
                id = "1F",
                name = "Fire Burn",
                power = PowerSelectionScript.Powers.Fire,
                damage = 5,
                range = 5,
                cooldown = 3,
                icon = abilityIconData.icons[1]
            });
            
            AddPower(new PowerSelectionScript.PowerSelection
            {
                level = 1,
                id = "1W",
                name = "Water Wave",
                power = PowerSelectionScript.Powers.Water,
                damage = 2,
                range = 8,
                cooldown = 5
            });
            
            AddPower(new PowerSelectionScript.PowerSelection
            {
                level = 1,
                id = "1W",
                name = "Water Squirt",
                power = PowerSelectionScript.Powers.Water,
                damage = 2,
                range = 8,
                cooldown = 3
            });
            
            AddPower(new PowerSelectionScript.PowerSelection
            {
                level = 1,
                id = "1E",
                name = "Earth Quake",
                power = PowerSelectionScript.Powers.Earth,
                damage = 2,
                range = 5,
                cooldown = 5
            });
            
            AddPower(new PowerSelectionScript.PowerSelection
            {
                level = 1,
                id = "1E",
                name = "Rock Throw",
                power = PowerSelectionScript.Powers.Earth,
                damage = 4,
                range = 4,
                cooldown = 8
            });
            
            AddPower(new PowerSelectionScript.PowerSelection
            {
                level = 1,
                id = "1A",
                name = "Air Slash",
                power = PowerSelectionScript.Powers.Air,
                damage = 3,
                range = 3,
                cooldown = 5
            });
            
            AddPower(new PowerSelectionScript.PowerSelection
            {
                level = 1,
                id = "1A",
                name = "Twister",
                power = PowerSelectionScript.Powers.Air,
                damage = 2,
                range = 4,
                cooldown = 3
            });
        }
        
        private void AddPower(PowerSelectionScript.PowerSelection power)
        {
            int countBefore = powerData.Count;
            powerData.Add(power);
            int countAfter = powerData.Count;
        
            if (countAfter <= countBefore)
            {
                Debug.LogWarning($"Failed to add power: {power.name} with ID: {power.id}");
            }
        }

        public PowerSelectionScript.PowerSelection FindPower(string powerID)
        {
            // Find the power with the matching ID
            var power = powerData.FirstOrDefault(x => x.id == powerID);

            // Check if power was found
            if (power.id != null) // Using default comparison for struct
            {
                return power;
            }
            else
            {
                Debug.Log("Power Not Found");
                return default;
            }
        }
    }
}
