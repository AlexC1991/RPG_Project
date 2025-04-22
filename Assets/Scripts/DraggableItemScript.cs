using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace RPGGame
{
    public class DraggableItemScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
       private RectTransform rectTransform;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private Vector2 originalPosition;
        public Transform originalParent;
        private ItemScript itemScript;
        private PlayerPowers playerPowers;
        private UIController uiController;
        private GameObject lastGameObject;
        private GameObject newGameObject;
        [TagSelector] [SerializeField] private string abilityTag;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            itemScript = GetComponentInParent<ItemScript>();
            playerPowers = FindObjectOfType<PlayerPowers>();
            uiController = FindObjectOfType<UIController>();

            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPosition = rectTransform.anchoredPosition;
            originalParent = transform.parent;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;

            // Check if dragging from ability slot
            if (originalParent.CompareTag(abilityTag))
            {
                // Clear the ability slot
                originalParent.GetComponent<Image>().sprite = null;
                originalParent.GetComponent<CanvasGroup>().alpha = 0f;
        
                // Remove from player powers
                int slotIndex = originalParent.GetSiblingIndex();
                if (slotIndex < playerPowers.playersPowers.Count)
                {
                    playerPowers.playersPowers.RemoveAt(slotIndex);
                }
            }

            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();

            if (itemScript != null && itemScript.itemI.itemData.isEquippable)
            {
                foreach (var slot in uiController.powerIcons)
                {
                    slot.GetComponent<CanvasGroup>().alpha = 0.8f;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            transform.position = Input.mousePosition;
            if (eventData.pointerEnter && eventData.pointerEnter.CompareTag(abilityTag))
            {
                if (eventData.pointerEnter.GetComponent<CanvasGroup>() != null) 
                {
                    newGameObject = eventData.pointerEnter.gameObject;
                    newGameObject.GetComponent<CanvasGroup>().alpha = 1;

                    if (lastGameObject == null)
                    {
                        lastGameObject = newGameObject;
                    }
                    else if (lastGameObject != newGameObject)
                    {
                        lastGameObject.GetComponent<CanvasGroup>().alpha = 0;
                        lastGameObject = newGameObject;
                    }
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(originalParent);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            newGameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}




