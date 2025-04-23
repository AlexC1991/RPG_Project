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
        private AbilityBarScript uiController;
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
            uiController = FindObjectOfType<AbilityBarScript>();

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
                // Clear the slot visuals
                originalParent.GetComponent<Image>().sprite = null;
                originalParent.GetComponent<CanvasGroup>().alpha = 0;

                // Hide the glow effect if it exists
                if (originalParent.childCount > 0)
                {
                    Transform glowChild = originalParent.GetChild(0);
                    if (glowChild != null && glowChild.GetComponent<CanvasGroup>() != null)
                    {
                        glowChild.GetComponent<CanvasGroup>().alpha = 0f;
                    }
                }

                // Remove from player powers
                int slotIndex = originalParent.GetSiblingIndex();
                if (slotIndex < playerPowers.playersPowers.Count)
                {
                    playerPowers.playersPowers.RemoveAt(slotIndex);
                }
            }

            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();

            // Show available ability slots with correct alpha
            if (itemScript != null && itemScript.itemI.itemData.isEquippable)
            {
                foreach (var slot in uiController.abilitySelection)
                {
                    slot.GetComponent<CanvasGroup>().alpha = 0.01f;
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

                    // The parent alpha stays at 0.01f when highlighted
                    newGameObject.GetComponent<CanvasGroup>().alpha = 1f;
                       
                    Transform glowChild = newGameObject.transform.GetChild(0);
                        
                    if (glowChild != null && glowChild.GetComponent<CanvasGroup>() != null)
                    {
                        glowChild.GetComponent<CanvasGroup>().alpha = 1f;
                    }
                    

                    // Handle previous slot - restore to original state
                    if (lastGameObject != null && lastGameObject != newGameObject)
                    {
                        // Return to default state (parent 0.01f, glow 0f)
                        lastGameObject.GetComponent<CanvasGroup>().alpha = 0.01f;

                        if (lastGameObject.transform.childCount > 0)
                        {
                            Transform lastGlow = lastGameObject.transform.GetChild(0);
                            if (lastGlow && lastGlow.GetComponent<CanvasGroup>())
                            {
                                lastGlow.GetComponent<CanvasGroup>().alpha = 0f;
                            }
                        }

                        lastGameObject = newGameObject;
                    }
                    else if (lastGameObject == null)
                    {
                        lastGameObject = newGameObject;
                    }
                }
            }
            
            else if (lastGameObject != null)
            {
                // When moving away from all ability slots, reset last one to default
                lastGameObject.GetComponent<CanvasGroup>().alpha = 0.01f;

                if (lastGameObject.transform.childCount > 0)
                {
                    Transform lastGlow = lastGameObject.transform.GetChild(0);
                    if (lastGlow && lastGlow.GetComponent<CanvasGroup>())
                    {
                        lastGlow.GetComponent<CanvasGroup>().alpha = 0f;
                    }
                }

                lastGameObject = null;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(originalParent);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;

            if (newGameObject != null && newGameObject.transform.childCount > 0)
            {
                Transform glowChild = newGameObject.transform.GetChild(0);
                if (glowChild && glowChild.GetComponent<CanvasGroup>())
                {
                    glowChild.GetComponent<CanvasGroup>().alpha = 0;
                }
            }
        }
    }
}




