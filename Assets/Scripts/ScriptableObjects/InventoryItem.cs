using UnityEngine;

namespace RPGGame
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "RPG Game SO/Inventory Item", order = 5)]
    public class InventoryItem : ScriptableObject
    {
        [System.Serializable]
        public struct TheInventoryItems
        {
            public string id;
            public string name;
            public string description;
            public Sprite icon;
            public GameObject itemPrefab;
            public int price;
            public int quantity;
            public bool isStackable;
            public bool isConsumable;
            public bool isEquippable;
            public bool isQuestItem;
        }

        public TheInventoryItems itemData;
    }
}