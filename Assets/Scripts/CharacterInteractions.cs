using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPGGame
{
    public class CharacterInteractions : MonoBehaviour
    {
        [TagSelector] [SerializeField] private string[] interactableTag;
        [SerializeField] private InputActionAsset controllerSettings;
        private UIController _uiC;
        private CharacterMovement _characterMovement;
        private InputAction _interact;
        [SerializeField] private GameObject storeUI;
        private bool _storeChecker;
        private int i;
        private CanvasGroup canvasG;

        private void Awake()
        {
            _uiC = FindObjectOfType<UIController>();
            canvasG = storeUI.GetComponent<CanvasGroup>();
            _interact = controllerSettings.FindActionMap("Player").FindAction("Interact");
        }

        private void OnEnable()
        {
            _interact.Enable();
        }
        
        private void Start()
        {
            _characterMovement = FindObjectOfType<CharacterMovement>();
            canvasG.alpha = 0;
            canvasG.blocksRaycasts = false;     
            _storeChecker = false;
            StartCoroutine(InteractionCheck());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(interactableTag[0]))
            {
                _storeChecker = true;
                Debug.Log("Can Interact With Store");
            }
        }

        private IEnumerator InteractionCheck()
        {
            while (true)
            {
                Debug.Log("UI Window Checker " + _uiC.uiWindowOpened);
                Debug.Log("interaction Number is " + i);
                if (_interact.triggered && _storeChecker)
                {
                    // Use the toggle method from UIController
                    if (canvasG.alpha < 0.5f)
                    {
                        // Open store UI
                        _uiC.OpenUIWindow(storeUI);
                        _characterMovement.StopCharacterMovement();
                        Cursor.visible = true;
                    }
                    else
                    {
                        // Close store UI
                        _uiC.CloseUIWindow(storeUI);
                        _characterMovement.StartCharacterMovement();
                        Cursor.visible = false;
                    }
                }
                yield return null;

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(interactableTag[0]))
            {
                _storeChecker = false;
                Debug.Log("Store is Closed");
                i = 0;
                storeUI.GetComponent<CanvasGroup>().alpha = 0;
                storeUI.GetComponent<CanvasGroup>().blocksRaycasts = false;  
                Debug.Log("Store UI Canvas Group Alpha is 0");
            }
        }

        private void OnDisable()
        {
            _interact.Disable();
        }
    }
}
