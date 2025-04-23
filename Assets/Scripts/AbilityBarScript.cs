using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RPGGame
{
    public class AbilityBarScript : MonoBehaviour, IDropHandler
    {
        [SerializeField] private InputActionAsset controllerSettings;
        public GameObject[] abilitySelection;
        [SerializeField] private GameObject abilityBarParent;
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

            // Set parent bar to visible
            abilityBarParent.GetComponent<CanvasGroup>().alpha = 1;

            // Reset all ability slots
            foreach (var g in abilitySelection)
            {
                g.GetComponent<CanvasGroup>().alpha = 0.01f;

                // Set all glow effects to invisible
                if (g.transform.childCount > 0)
                {
                    Transform glowChild = g.transform.GetChild(0);
                    if (glowChild != null && glowChild.GetComponent<CanvasGroup>() != null)
                    {
                        glowChild.GetComponent<CanvasGroup>().alpha = 0f;
                    }
                }
            }

            // Start coroutines
            _uiUsage = StartCoroutine(UIFunctions());

            // Initialize with the ability bar already visible
            inventoryOpen = true;

            // Check if we have any abilities to display
            CheckAbilityIcons();
        }


        private void OnEnable()
        {
            abilityOne.performed += OnAbilityOnePerformed;
            abilityTwo.performed += OnAbilityTwoPerformed;
            abilityThree.performed += OnAbilityThreePerformed;
            abilityFour.performed += OnAbilityFourPerformed;
            abilityFive.performed += OnAbilityFivePerformed;
            
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
                // No longer toggle visibility with openInventory.triggered
                // Just monitor for inconsistent state
                if (abilityBarParent.GetComponent<CanvasGroup>().alpha < 0.5f)
                {
                    inventoryOpen = false;
                    uiWindowOpened = false;
                }
                else
                {
                    inventoryOpen = true;
                }

                yield return null;
            }
        }

        private void CheckAbilityIcons()
        {
            // Check each ability slot
            for (int i = 0; i < abilitySelection.Length; i++)
            {
                GameObject ability = abilitySelection[i];

                // Check if we have a power assigned to this slot
                if (i < pP.playersPowers.Count)
                {
                    // Update the image with the power's icon
                    ability.GetComponent<Image>().sprite = pP.playersPowers[i].icon;

                    // Set proper alpha for parent (0.01f)
                    ability.GetComponent<CanvasGroup>().alpha = 0.01f;

                    // Set glow child if it exists
                    if (ability.transform.childCount > 0)
                    {
                        Transform glowChild = ability.transform.GetChild(0);
                        if (glowChild != null && glowChild.GetComponent<CanvasGroup>() != null)
                        {
                            // Initially hide glow
                            glowChild.GetComponent<CanvasGroup>().alpha = 0f;
                        }
                    }
                }
                else
                {
                    // No power assigned, reset to default state
                    ability.GetComponent<Image>().sprite = null;
                    ability.GetComponent<CanvasGroup>().alpha = 0f;

                    if (ability.transform.childCount > 0)
                    {
                        Transform glowChild = ability.transform.GetChild(0);
                        if (glowChild != null && glowChild.GetComponent<CanvasGroup>() != null)
                        {
                            glowChild.GetComponent<CanvasGroup>().alpha = 0f;
                        }
                    }
                }
            }
        }

        private void OnAbilityOnePerformed(InputAction.CallbackContext context)
        {
            if (pP.playersPowers.Count > 0)
            {
                HighlightAbility(0);
                abilityActivation.ActivateTheAbility(pP.playersPowers[0].id);
            }
        }

        private void OnAbilityTwoPerformed(InputAction.CallbackContext context)
        {
            if (pP.playersPowers.Count > 1)
            {
                HighlightAbility(1);
                abilityActivation.ActivateTheAbility(pP.playersPowers[1].id);
            }
        }

        private void OnAbilityThreePerformed(InputAction.CallbackContext context)
        {
            if (pP.playersPowers.Count > 2)
            {
                HighlightAbility(2);
                abilityActivation.ActivateTheAbility(pP.playersPowers[2].id);
            }
        }

        private void OnAbilityFourPerformed(InputAction.CallbackContext context)
        {
            if (pP.playersPowers.Count > 3)
            {
                HighlightAbility(3);
                abilityActivation.ActivateTheAbility(pP.playersPowers[3].id);
            }
        }

        private void OnAbilityFivePerformed(InputAction.CallbackContext context)
        {
            if (pP.playersPowers.Count > 4)
            {
                HighlightAbility(4);
                abilityActivation.ActivateTheAbility(pP.playersPowers[4].id);
            }
        }

        private void HighlightAbility(int index)
        {
            if (index >= 0 && index < abilitySelection.Length)
            {
                // Set parent alpha to 0.01 (not completely transparent)
                abilitySelection[index].GetComponent<CanvasGroup>().alpha = 1f;

                // Activate glow effect
                if (abilitySelection[index].transform.childCount > 0)
                {
                    Transform glowChild = abilitySelection[index].transform.GetChild(0);
                    if (glowChild != null && glowChild.GetComponent<CanvasGroup>() != null)
                    {
                        glowChild.GetComponent<CanvasGroup>().alpha = 1f;
                    }
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
            // This method is kept for compatibility but modified to keep the main UI visible

            // Hide all ability selection highlights
            foreach (var ability in abilitySelection)
            {
                ability.GetComponent<CanvasGroup>().alpha = 0;

                // Also hide the glow effect
                if (ability.transform.childCount > 0)
                {
                    Transform glowChild = ability.transform.GetChild(0);
                    if (glowChild != null && glowChild.GetComponent<CanvasGroup>() != null)
                    {
                        glowChild.GetComponent<CanvasGroup>().alpha = 0f;
                    }
                }
            }

            // Reset global UI state but keep main bar visible
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
            abilityOne.performed -= OnAbilityOnePerformed;
            abilityTwo.performed -= OnAbilityTwoPerformed;
            abilityThree.performed -= OnAbilityThreePerformed;
            abilityFour.performed -= OnAbilityFourPerformed;
            abilityFive.performed -= OnAbilityFivePerformed;
            
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
                // Debug which slot is highlighted
                Debug.Log("Checking slots for drop - looking for alpha = 1");

                for (int i = 0; i < abilitySelection.Length; i++)
                {
                    // The slot we're hovering over should have alpha = 1 (fully visible)
                    float slotAlpha = abilitySelection[i].GetComponent<CanvasGroup>().alpha;
                    Debug.Log($"Slot {i} alpha: {slotAlpha}");

                    if (slotAlpha >= 0.9f) // Changed from 0.01f to 0.9f to detect highlighted slots
                    {
                        // Place the item in this slot
                        Debug.Log($"Placing item in slot {i}");

                        // Set the dragged item as a child of the ability slot
                        droppedGO.transform.SetParent(abilitySelection[i].transform);
                        droppedGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                        // Update visuals
                        abilitySelection[i].GetComponent<Image>().sprite =
                            draggedItem.GetComponentInParent<ItemScript>().itemI.itemData.icon;

                        // Return parent to semi-transparent state
                        abilitySelection[i].GetComponent<CanvasGroup>().alpha = 1f;

                        // Reset glow effect
                        if (abilitySelection[i].transform.childCount > 0)
                        {
                            Transform glowChild = abilitySelection[i].transform.GetChild(0);
                            if (glowChild && glowChild.GetComponent<CanvasGroup>())
                            {
                                glowChild.GetComponent<CanvasGroup>().alpha = 0f;
                            }
                        }

                        // Add to player powers and update
                        PowerSelectionScript.PowerSelection newPower = new PowerSelectionScript.PowerSelection
                        {
                            // Set power properties from item
                            icon = draggedItem.GetComponentInParent<ItemScript>().itemI.itemData.icon,
                            id = draggedItem.GetComponentInParent<ItemScript>().itemI.itemData.id
                        };

                        if (i < pP.playersPowers.Count)
                        {
                            pP.playersPowers[i] = newPower;
                        }
                        else
                        {
                            pP.playersPowers.Add(newPower);
                        }

                        return;
                    }
                }
            }

            Debug.Log("Drop failed - no valid target or item not equippable");
        }

    }

}


    



    

