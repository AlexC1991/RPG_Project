using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RPGGame
{
    public class UIController : MonoBehaviour, IDropHandler
    {
        [SerializeField] private InputActionAsset controllerSettings;
        [SerializeField] private GameObject[] uiElements;
        [SerializeField] public GameObject[] powerIcons;
        public GameObject[] abilitySelection;
        [SerializeField] PlayerPowers pP;
        private AbilityActivation abilityActivation;
        private InputAction openInventory;
        private InputAction abilityOne;
        private InputAction abilityTwo;
        private InputAction abilityThree;
        private InputAction abilityFour;
        private InputAction abilityFive;
        private Coroutine _uiUsage;
        private bool inventoryOpen;
        private int i = 0;
        public bool uiWindowOpened;
        private GameObject currentOpenWindow;
        private Coroutine _actionBar;
        private GameObject powerI;


        private void Awake()
        { 
            openInventory = controllerSettings.FindActionMap("Player").FindAction("Inventory");
            abilityOne = controllerSettings.FindActionMap("Player").FindAction("AbilitySelectionOne");
            abilityTwo = controllerSettings.FindActionMap("Player").FindAction("AbilitySelectionTwo");
            abilityThree = controllerSettings.FindActionMap("Player").FindAction("AbilitySelectionThree");
            abilityFour = controllerSettings.FindActionMap("Player").FindAction("AbilitySelectionFour");
            abilityFive = controllerSettings.FindActionMap("Player").FindAction("AbilitySelectionFive");
        }

        private void Start()
        {
            abilityActivation = FindObjectOfType<AbilityActivation>();
            
            foreach (var g in uiElements)
            {
                g.GetComponent<CanvasGroup>().alpha = 0;
            }
            
            foreach (var g in abilitySelection)
            {
                g.GetComponent<CanvasGroup>().alpha = 0;
            }
            
            _uiUsage = StartCoroutine(UIFunctions());
        }
       
      
        private void OnEnable()
        {
            openInventory.Enable();
            abilityOne.Enable();
            abilityTwo.Enable();
            abilityThree.Enable();
            abilityFour.Enable();
            abilityFive.Enable();
        }
       
        private IEnumerator UIFunctions()
        {
            while (true)
            {
                // Check for inventory toggle request
                if (openInventory.triggered)
                {
                    // Toggle inventory state only if no UI is open, or if this inventory is already open
                    if (!uiWindowOpened || inventoryOpen)
                    {
                        inventoryOpen = !inventoryOpen;
                        // Update UI visibility based on new state
                        if (inventoryOpen)
                        {
                            // Open inventory UI
                            uiElements[0].GetComponent<CanvasGroup>().alpha = 1;
                            uiElements[0].GetComponent<CanvasGroup>().blocksRaycasts = true;
                            Cursor.visible = true;
                            uiWindowOpened = true;
                    
                            // Initialize inventory content
                            CheckPowerIcons();
                            _actionBar = StartCoroutine(ActionBar());
                            Debug.Log("Inventory Opened");
                        }
                        else
                        {
                            // Close inventory UI
                            StopCoroutine(_actionBar);
                            CloseInventory();
                            Debug.Log("Inventory Closed");
                        }
                    }
                }
        
                // Check for inconsistent state (uiWindowOpened flag is true but inventory is invisible)
                if (uiWindowOpened && inventoryOpen && uiElements[0].GetComponent<CanvasGroup>().alpha < 0.5f)
                {
                    inventoryOpen = false;
                    uiWindowOpened = false;
                }
        
                yield return null;
            }
        }

        private IEnumerator ActionBar()
        {
            while (true)
            {
                ChooseAbilitySelection();
                yield return null;
            }
        }

        private void CheckPowerIcons()
        {
            foreach (var g in powerIcons)
            {
                if (g.GetComponent<CanvasGroup>().alpha == 0)
                {
                    if (g.GetComponent<Image>().sprite != null)
                    {
                        g.GetComponent<CanvasGroup>().alpha = 1;
                    }
                }
            }
        }

        private void ChooseAbilitySelection()
        {
            if (abilityOne.triggered)
            {
                abilitySelection[0].GetComponent<CanvasGroup>().alpha = 1;
                abilitySelection[1].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[2].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[3].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[4].GetComponent<CanvasGroup>().alpha = 0;
                if (pP.playersPowers.Count > 0)
                {
                    abilityActivation.ActivateTheAbility(pP.playersPowers[0].id);
                }
            }
            if (abilityTwo.triggered)
            {
                abilitySelection[0].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[1].GetComponent<CanvasGroup>().alpha = 1;
                abilitySelection[2].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[3].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[4].GetComponent<CanvasGroup>().alpha = 0;
                if (pP.playersPowers.Count > 1)
                {
                    abilityActivation.ActivateTheAbility(pP.playersPowers[1].id);
                }
            }
            if (abilityThree.triggered)
            {
                abilitySelection[0].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[1].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[2].GetComponent<CanvasGroup>().alpha = 1;
                abilitySelection[3].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[4].GetComponent<CanvasGroup>().alpha = 0;
                if (pP.playersPowers.Count > 2)
                {
                    abilityActivation.ActivateTheAbility(pP.playersPowers[2].id);
                }
            }
            if (abilityFour.triggered)
            {
                abilitySelection[0].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[1].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[2].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[3].GetComponent<CanvasGroup>().alpha = 1;
                abilitySelection[4].GetComponent<CanvasGroup>().alpha = 0;
                if (pP.playersPowers.Count > 3)
                {
                    abilityActivation.ActivateTheAbility(pP.playersPowers[3].id);
                }
            }
            if (abilityFive.triggered)
            {
                abilitySelection[0].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[1].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[2].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[3].GetComponent<CanvasGroup>().alpha = 0;
                abilitySelection[4].GetComponent<CanvasGroup>().alpha = 1;
                if (pP.playersPowers.Count > 4)
                {
                    abilityActivation.ActivateTheAbility(pP.playersPowers[4].id);
                }
            }
        }
        
        public void OpenUIWindow(GameObject uiWindow)
        {
            // If a window is already open, close it first
            if (uiWindowOpened && currentOpenWindow != null && currentOpenWindow != uiWindow)
            {
                CloseUIWindow(currentOpenWindow);
            }
        
            // Open the new window
            CanvasGroup canvasGroup = uiWindow.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        
            currentOpenWindow = uiWindow;
            uiWindowOpened = true;
        }
    
        // Method to close a specific UI window
        public void CloseUIWindow(GameObject uiWindow)
        {
            CanvasGroup canvasGroup = uiWindow.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        
            if (currentOpenWindow == uiWindow)
            {
                currentOpenWindow = null;
                uiWindowOpened = false;
            }
        }
        
        private void CloseInventory()
        {
            // Hide main inventory UI
            uiElements[0].GetComponent<CanvasGroup>().alpha = 0;
            uiElements[0].GetComponent<CanvasGroup>().blocksRaycasts = false;
    
            // Hide all ability selection UIs
            foreach (var selection in abilitySelection)
            {
                selection.GetComponent<CanvasGroup>().alpha = 0;
                selection.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
    
            // Reset global UI state
            uiWindowOpened = false;
            Cursor.visible = false;
        }

    
        // Toggle method - opens if closed, closes if open
        public void ToggleUIWindow(GameObject uiWindow)
        {
            CanvasGroup canvasGroup = uiWindow.GetComponent<CanvasGroup>();
            if (canvasGroup == null) return;
        
            if (canvasGroup.alpha < 0.5f)
            {
                OpenUIWindow(uiWindow);
            }
            else
            {
                CloseUIWindow(uiWindow);
            }
        }

        
        private void OnDisable()
        {
            openInventory.Disable();
            abilityOne.Disable();
            abilityTwo.Disable();
            abilityThree.Disable();
            abilityFour.Disable();
            abilityFive.Disable();
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject droppedGO = eventData.pointerDrag;
            DraggableItemScript draggedItem = droppedGO.GetComponent<DraggableItemScript>();

            if (draggedItem != null && draggedItem.GetComponentInParent<ItemScript>().itemI.itemData.isEquippable)
            {
                for (int i = 0; i < abilitySelection.Length; i++)
                {
                    if (abilitySelection[i].GetComponent<CanvasGroup>().alpha > 0)
                    {
                        // Clear previous item if exists
                        if (powerIcons[i].transform.childCount > 0)
                        {
                            Destroy(powerIcons[i].transform.GetChild(0).gameObject);
                        }

                        // Set the dragged item as a child of the power icon
                        droppedGO.transform.SetParent(powerIcons[i].transform);
                        droppedGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                        // Update visuals
                        powerIcons[i].GetComponent<Image>().sprite =
                            draggedItem.GetComponentInParent<ItemScript>().itemI.itemData.icon;
                        powerIcons[i].GetComponent<CanvasGroup>().alpha = 1f;

                        // Add to player powers
                        PowerSelectionScript.PowerSelection newPower = new PowerSelectionScript.PowerSelection
                        {
                            id = draggedItem.GetComponentInParent<ItemScript>().itemI.itemData.id,
                            icon = draggedItem.GetComponentInParent<ItemScript>().itemI.itemData.icon
                        };

                        if (i < pP.playersPowers.Count)
                        {
                            pP.playersPowers[i] = newPower;
                        }
                        else
                        {
                            pP.playersPowers.Add(newPower);
                        }

                        break;
                    }
                }
            }
        }
    }
}
