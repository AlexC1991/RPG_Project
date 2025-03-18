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
        private CharacterMovement _characterMovement;
        private InputAction _interact;
        [SerializeField] private GameObject storeUI;
        private bool _storeChecker;
        private int i;

        private void Awake()
        {
            _interact = controllerSettings.FindActionMap("Player").FindAction("Interact");
        }

        private void OnEnable()
        {
            _interact.Enable();
        }
        
        private void Start()
        {
            _characterMovement = FindObjectOfType<CharacterMovement>();
            storeUI.GetComponent<CanvasGroup>().alpha = 0;
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
                Debug.Log("interaction Number is " + i);
                if (_interact.triggered)
                {
                    if (_storeChecker)
                    {
                        i += 1;
                    }
                    
                    if (_storeChecker && i % 2 != 0)
                    {
                        storeUI.GetComponent<CanvasGroup>().alpha = 1;
                        Debug.Log("Store UI Canvas Group Alpha is 1");
                        _characterMovement.StopCharacterMovement();
                        Cursor.visible = true;
                    }

                    if (i % 2 == 0)
                    {
                        storeUI.GetComponent<CanvasGroup>().alpha = 0;
                        Debug.Log("Store UI Canvas Group Alpha is 0");
                        _characterMovement.StartCharacterMovement();
                        Cursor.visible = false;
                    }

                    if (i > 2)
                    {
                        i = 0;
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
                Debug.Log("Store UI Canvas Group Alpha is 0");
            }
        }

        private void OnDisable()
        {
            _interact.Disable();
        }
    }
}
