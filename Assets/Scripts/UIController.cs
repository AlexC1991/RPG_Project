using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RPGGame
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset controllerSettings;
        [SerializeField] private GameObject[] uiElements;
        [SerializeField] public GameObject[] powerIcons;
        [SerializeField] private GameObject[] abilitySelection;
        [SerializeField] PlayerPowers pP;
        private AbilityActivation abilityActivation;
        private InputAction openInventory;
        private InputAction abilityOne;
        private InputAction abilityTwo;
        private InputAction abilityThree;
        private InputAction abilityFour;
        private InputAction abilityFive;
        private Coroutine _uiUsage;
        private int i = 0;
        
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
                if (openInventory.triggered)
                {
                    i += 1;
                    Debug.Log("Inventory Opened");
                }

                if (i == 1)
                {
                    uiElements[0].GetComponent<CanvasGroup>().alpha = 1;
                    Cursor.visible = true;
                    CheckPowerIcons();
                    ChooseAbilitySelection();
                }
                else
                {
                    uiElements[0].GetComponent<CanvasGroup>().alpha = 0;
                }
                if (i > 1)
                {
                    Cursor.visible = false;
                    abilitySelection[0].GetComponent<CanvasGroup>().alpha = 0;
                    abilitySelection[1].GetComponent<CanvasGroup>().alpha = 0;
                    abilitySelection[2].GetComponent<CanvasGroup>().alpha = 0;
                    abilitySelection[3].GetComponent<CanvasGroup>().alpha = 0;
                    abilitySelection[4].GetComponent<CanvasGroup>().alpha = 0;
                    
                    i = 0;
                }
                
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
        
        private void OnDisable()
        {
            openInventory.Disable();
            abilityOne.Disable();
            abilityTwo.Disable();
            abilityThree.Disable();
            abilityFour.Disable();
            abilityFive.Disable();
        }
    }
}
