using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteractions : MonoBehaviour
{
    [TagSelector][SerializeField] private string[] interactableTag;
    [SerializeField] private InputActionAsset controllerSettings;
    private InputAction _interact;
    [SerializeField] private GameObject storeUI;
    private bool _storeChecker;
    private void Awake()
    {
        _interact = controllerSettings.FindActionMap("Player").FindAction("Interact");
    }

    private void onEnable()
    {
        _interact.Enable();
    }

    private void Start()
    {
        storeUI.GetComponent<CanvasGroup>().alpha = 0;
        _storeChecker = false;
        StartCoroutine(InteractionCheck());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Array.Exists(interactableTag, tag => tag == other.tag))
        {
            if (other.tag == interactableTag[0])
            {
                _storeChecker = true;
                Debug.Log("Can Interact With Store");
            }
        }
    }

    private IEnumerator InteractionCheck()
    {
        while (true)
        {
            if (_interact.triggered && _storeChecker)
            {
                if (storeUI.GetComponent<CanvasGroup>().alpha != 1)
                {
                    storeUI.GetComponent<CanvasGroup>().alpha = 1;
                }
                else
                {
                    storeUI.GetComponent<CanvasGroup>().alpha = 0;
                }
            }
            yield return null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Array.Exists(interactableTag, tag => tag == other.tag))
        {
            if (other.tag == interactableTag[0])
            {
                _storeChecker = false;
                Debug.Log("Store is Closed");
            }
        }
    }

    private void OnDisable()
    {
        _interact.Disable();
    }
}
