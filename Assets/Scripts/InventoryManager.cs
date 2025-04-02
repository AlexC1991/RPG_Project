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
        [SerializeField] private UIController uiC;
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject uiParent;
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

        public void AddToInventory(GameObject item, int amount, Sprite icon)
        {
            // Check if inventory is full
            if (inventory.Count >= inventorySize)
            {
                Debug.Log("Inventory is full");
                return;
            }

            int slotIndex = -1;

            // Check for empty slots first
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i] == null)
                {
                    slotIndex = i;
                    Debug.Log($"Found empty slot at index {i}");
                    break;
                }
            }

            // If no empty slots, use the next available index
            if (slotIndex == -1)
            {
                slotIndex = inventory.Count;
                Debug.Log($"No empty slots, using next available index: {slotIndex}");
            }

            // Calculate grid position based on slot index, not inventory count
            int row = slotIndex / 3;
            int column = slotIndex % 3;

            // Determine position based on grid coordinates
            float xPosition;
            if (column == 0) xPosition = -33f;
            else if (column == 1) xPosition = 0f;
            else xPosition = 32f;

            float yPosition = 38f - (row * 25f);

            Debug.Log(
                $"Positioning item at slot {slotIndex}: Row {row}, Column {column}, Position: ({xPosition}, {yPosition})");

            // Create and position the UI item
            GameObject uiItem = Instantiate(inventoryPrefab, uiParent.transform);
            uiItem.name = $"item_{slotIndex}"; // More consistent naming
            RectTransform rectTransform = uiItem.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);

            // Configure item appearance
            subParent = uiItem.transform.GetChild(0).transform.GetChild(0).gameObject;
            subParent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + amount.ToString("00");
            subParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.name;
            subParent.GetComponent<Image>().sprite = icon;

            // Update inventory data
            if (slotIndex < inventory.Count)
            {
                // Replace existing null slot
                inventory[slotIndex] = item;
                inventoryUIItems[slotIndex] = uiItem;
                Debug.Log($"Replaced null item at index {slotIndex}");
            }
            else
            {
                // Add to the end of the lists
                inventory.Add(item);
                inventoryUIItems.Add(uiItem);
                Debug.Log($"Added new item at index {slotIndex}, inventory size now: {inventory.Count}");
            }

            // Always destroy the original item
            CharacterInteractions characterI = FindObjectOfType<CharacterInteractions>();
            if (characterI != null)
            {
                characterI.DestroyItem(item);
            }
            else
            {
                Debug.LogWarning("CharacterInteractions not found, could not destroy item");
            }
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
