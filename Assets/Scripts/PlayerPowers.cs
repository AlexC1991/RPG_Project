using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame
{
    public class PlayerPowers : MonoBehaviour
    {
        public List<PowerSelectionScript.PowerSelection> playersPowers = new List<PowerSelectionScript.PowerSelection>();
        [SerializeField] private UIController uiC;
        private void Start()
        {
            playersPowers.Clear();
        }

        public void UpdateInventoryBar()
        {
            for (int i = 0; i < playersPowers.Count && i < uiC.powerIcons.Length; i++)
            {
                uiC.powerIcons[i].GetComponent<Image>().sprite = playersPowers[i].icon;
            }
        }
    }
}
