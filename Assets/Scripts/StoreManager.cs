using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame
{
    public class StoreManager : MonoBehaviour
    {
        [SerializeField] private AbilityContainer aC;
        [SerializeField] private Image[] displayedIcon;
        [SerializeField] private TextMeshProUGUI[] displayedName;

        private void Start()
        {
            if (aC.powerSelectionScript.Length > 0)
            {
                for (int i = 0; i < aC.powerSelectionScript.Length; i++)
                {
                    displayedIcon[i].sprite = aC.powerSelectionScript[i].powerData.icon;
                    displayedName[i].text = aC.powerSelectionScript[i].powerData.name;
                }
            }
        }
    }
}
