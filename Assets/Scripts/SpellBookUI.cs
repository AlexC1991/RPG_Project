using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RPGGame
{
    public class SpellBookUI : MonoBehaviour
    {
        [SerializeField] private GameObject spellTemplatePrefab;
        [SerializeField] private Transform spellBookContent;
        private UIController _uiC;
        [SerializeField] private AbilityContainer[] aC;
        [TagSelector] [SerializeField] private string fireTag;
        [TagSelector] [SerializeField] private string waterTag;
        [TagSelector] [SerializeField] private string earthTag;
        [TagSelector] [SerializeField] private string airTag;
        public List<PowerSelectionScript.PowerSelection> powers = new List<PowerSelectionScript.PowerSelection>();
        private CanvasGroup canvasG;
        [SerializeField] private InputActionAsset controllerSettings;
        [SerializeField] private InputActionReference spellActionReference;


        private void Awake()
        {
            _uiC = FindObjectOfType<UIController>();
            canvasG = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            canvasG.alpha = 0;
            canvasG.blocksRaycasts = false;
            canvasG.interactable = false;
        }

        private void OnEnable()
        {
            if (spellActionReference != null)
            {
                spellActionReference.action.performed += OnSpellActionPerformed;
                spellActionReference.action.Enable();
            }
        }

        private void OnSpellActionPerformed(InputAction.CallbackContext context)
        {

            if (context.performed)
            {
                // Get reference to the UI GameObject (assuming this script is attached to it)
                GameObject spellBookUI = this.gameObject;

                // Use the centralized UI system
                _uiC.ToggleUIWindow(spellBookUI);

                // Additional logic when opened
                if (spellBookUI.GetComponent<CanvasGroup>().alpha > 0.5f)
                {
                    // Set up spell book content, stop movement, etc.
                    SetSpellBook();
                    FindObjectOfType<CharacterMovement>()?.StopCharacterMovement();
                    Cursor.visible = true;
                }
                else
                {
                    // Cleanup when closed
                    FindObjectOfType<CharacterMovement>()?.StartCharacterMovement();
                    Cursor.visible = false;
                }
            }

        }

        /// <summary>
        /// Updates the list of selected powers based on the tag of the provided button and configures the spell book accordingly.
        /// </summary>
        /// <param name="button">The button object whose tag is used to determine the selected power category.</param>
        public void SelectedPower(Button button)
        {
            powers.Clear();

            if (button.transform.tag == fireTag)
            {
                foreach (var ability in aC)
                {
                    if (ability.powerTag == fireTag)
                    {
                        foreach (var g in ability.powerSelectionScript)
                        {
                            powers.Add(g.powerData);
                            SetSpellBook();
                        }
                    }
                }
            }
            else if (button.transform.tag == waterTag)
            {
                foreach (var ability in aC)
                {
                    if (ability.powerTag == waterTag)
                    {
                        foreach (var g in ability.powerSelectionScript)
                        {
                            powers.Add(g.powerData);
                            SetSpellBook();
                        }
                    }
                }
            }
            else if (button.transform.tag == earthTag)
            {
                foreach (var ability in aC)
                {
                    if (ability.powerTag == earthTag)
                    {
                        foreach (var g in ability.powerSelectionScript)
                        {
                            powers.Add(g.powerData);
                            SetSpellBook();
                        }
                    }
                }
            }
            else if (button.transform.tag == airTag)
            {
                foreach (var ability in aC)
                {
                    if (ability.powerTag == airTag)
                    {
                        foreach (var g in ability.powerSelectionScript)
                        {
                            powers.Add(g.powerData);
                            SetSpellBook();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the Spell Book UI by populating it with the current list of selected powers.
        /// </summary>
        /// <remarks>
        /// This method creates visual representations of each power in the spell book by instantiating a predefined template.
        /// It sets the icon, name, and price for each power based on the associated data in the list of powers.
        /// If the number of instantiated entries exceeds the number of powers in the list, old entries are removed from the UI.
        /// </remarks>
        private void SetSpellBook()
        {
            foreach (var power in powers)
            {
                GameObject spell = Instantiate(spellTemplatePrefab, spellBookContent);
                spell.GetComponent<SpellCardDetails>().powerForThisSpell = power;
                spell.transform.GetChild(0).GetComponent<Image>().sprite = power.icon;
                spell.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = power.name;
                spell.transform.name = power.name + " Spell";


                if (power.abilityPrice == 0)
                {
                    spell.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Free";
                }
                else
                {
                    spell.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                        power.abilityPrice.ToString("F2");
                }

                if (spellBookContent.childCount > powers.Count)
                {
                    Destroy(spellBookContent.GetChild(0).gameObject);
                }
            }
        }

        private void OnDisable()
        {
            if (spellActionReference != null)
            {
                spellActionReference.action.performed -= OnSpellActionPerformed;
                spellActionReference.action.Disable();
            }
        }
    }
}
            

        
    

