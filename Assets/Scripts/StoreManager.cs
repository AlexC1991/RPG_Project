using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame
{
    public class StoreManager : MonoBehaviour
    {
        [SerializeField] private AbilityContainer aC;
        public void BoughtPower(Button button)
        {
            Debug.Log("Bought Power");
            button.interactable = false;
            button.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
    }
}
