using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame
{
    public class PlayerPowers : MonoBehaviour
    {
        public List<PowerSelectionScript.PowerSelection> playersPowers = new List<PowerSelectionScript.PowerSelection>();
        [TagSelector] [SerializeField] private string powerBox;
        [SerializeField] private Image displayedIcon;
        private void Start()
        {
            playersPowers.Clear();
        }
    }
}
