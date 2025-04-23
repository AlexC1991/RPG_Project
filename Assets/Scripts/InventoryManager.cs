using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RPGGame
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryPrefab;
        [SerializeField] private AbilityBarScript uiC;
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject uiParent;
        [SerializeField] private CharacterInteractions cInteraction;
        private GameObject subParent;
        private GameObject uiItem;
        public List<GameObject> inventory = new List<GameObject>();
        public List<GameObject> inventoryUIItems = new List<GameObject>();
        private int inventorySize = 15;
        [SerializeField] private InputActionReference inventoryActionReference;
        private CanvasGroup canvasG;
        private Vector2 inventoryPosition;
        private void Start()
        {
            canvasG = inventoryUI.GetComponent<CanvasGroup>();
            canvasG.alpha = 0;
            canvasG.blocksRaycasts = false;
            canvasG.interactable = false;
            uiC.CloseUIWindow(inventoryUI);
            inventory.Clear();
        }
        
        private void OnEnable()
        {
            if (inventoryActionReference != null)
            {
                inventoryActionReference.action.performed += OnInventoryActionPerformed;
                inventoryActionReference.action.Enable();
            }
        }
        
        public IEnumerator AddToInventory(GameObject item, int amount, Sprite icon)
        {
            if (inventory.Count >= inventorySize)
            {
                Debug.Log("Inventory Full");
                yield break;
            }

            ItemScript currentItem = item.GetComponent<ItemScript>();

            // Check for existing stackable item
            for (int i = 0; i < inventory.Count; i++)
            {
                if (currentItem.itemI.itemData.itemPrefab.name == inventory[i].name &&
                    currentItem.itemI.itemData.isStackable)
                {
                    Debug.Log("Detecting Already Have It & Its Stackable");
                    // Only update the quantity
                    inventory[i].GetComponent<ItemScript>().itemI.itemData.quantity += amount;
                    inventoryUIItems[i].transform.GetChild(0).GetChild(0).GetChild(0)
                            .GetComponent<TextMeshProUGUI>().text =
                        inventory[i].GetComponent<ItemScript>().itemI.itemData.quantity.ToString("00");
                    cInteraction.DestroyItem(item);
                    yield break;
                }
            }
            
            Debug.Log("Its Not There Yet");
            // Calculate grid position
            int currentSlot = inventory.Count;
            int row = currentSlot / 3;     // 0, 0, 0, 1, 1, 1, 2, 2, 2
            int col = currentSlot % 3;     // 0, 1, 2, 0, 1, 2, 0, 1, 2

            // Calculate position
            float xPos = -33f + (col * 33.5f);  // -33, 0, 34
            float yPos = 38f - (row * 30f);     // 38, 8, -22

            // Create and position UI
            GameObject newUI = Instantiate(inventoryPrefab, uiParent.transform);
            RectTransform rectTransform = newUI.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(xPos, yPos);
           
            // Add and setup ItemScript
            ItemScript newItemScript = newUI.AddComponent<ItemScript>();
            newItemScript.itemI = ScriptableObject.CreateInstance<InventoryItem>();
            newItemScript.itemI.itemData = currentItem.itemI.itemData;
            
            // Add to lists
            inventory.Add(newUI);
            inventoryUIItems.Add(newUI);
            
            // Setup UI elements
            Transform itemParent = newUI.transform.GetChild(0);
            Transform iParent = itemParent.transform.GetChild(0);
            iParent.GetComponent<Image>().sprite = icon;
            iParent.GetChild(0).GetComponent<TextMeshProUGUI>().text = amount.ToString("00");
            iParent.GetChild(1).GetComponent<TextMeshProUGUI>().text = 
                currentItem.itemI.itemData.itemPrefab.name;

            cInteraction.DestroyItem(item);
            yield break;
        }
        
        private void OnInventoryActionPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Use the centralized UI system
                uiC.ToggleUIWindow(inventoryUI);

                // Additional logic when opened
                if (inventoryUI.GetComponent<CanvasGroup>().alpha > 0.9f)
                {
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.visible = false;
                }
            }
        }
        private void OnDisable()
        {
            if (inventoryActionReference != null)
            {
                inventoryActionReference.action.performed -= OnInventoryActionPerformed;
                inventoryActionReference.action.Disable();
            }
        }
        
    }
}
