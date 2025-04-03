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

        public IEnumerator AddToInventory(GameObject item, int amount, Sprite icon)
        {

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
