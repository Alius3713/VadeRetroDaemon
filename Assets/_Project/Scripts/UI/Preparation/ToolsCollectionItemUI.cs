using _Project.Scripts.Core.Data.Preparation;
using _Project.Scripts.Core.Data.Tools;
using _Project.Scripts.Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Preparation
{
    public class ToolsCollectionItemUI : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject selectedFrame;
        [SerializeField] private GameObject inLoadoutMarker;

        private ToolDefinition _tool;
        private PreparationPanelUI _owner;
        
        private float _lastClickTime;
        private const float DoubleClickThreshold = 0.3f;
        
        public ToolDefinition Tool => _tool;

        public void Initialize(ToolDefinition tool, PreparationPanelUI owner, bool isSelected, bool isInLoadout)
        {
            _tool = tool;
            _owner = owner;
            
            if (iconImage != null)
            {
                iconImage.sprite = _tool != null && _tool.Icon != null ? _tool.Icon : null;
                iconImage.enabled = _tool != null && _tool.Icon != null;
            }

            SetSelectedVisual(isSelected);
            SetInLoadoutVisual(isInLoadout);
        }

        public void SetSelectedVisual(bool isSelected)
        {
            if (selectedFrame != null) selectedFrame.SetActive(isSelected);
        }

        public void SetInLoadoutVisual(bool isInLoadout)
        {
            if (inLoadoutMarker != null) inLoadoutMarker.SetActive(isInLoadout);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_owner == null || _tool == null) return;
            
            _owner.ShowTooltip(_tool, eventData.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_owner == null) return;

            _owner.HideTooltip();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_owner == null || _tool == null) return;

            if (Time.unscaledTime - _lastClickTime <= DoubleClickThreshold)
            {
                TryRemoveFromLoadoutByDoubleClick();
                _lastClickTime = 0f;
                return;
            }
            
            _lastClickTime = Time.unscaledTime;
            _owner.HandleCollectionToolClicked(_tool);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_tool == null) return;
            
            ToolDragPayload.Set(_tool, -1);

            if (iconImage != null)
            {
                ToolDragVisualController.Instance?.Show(_tool, iconImage.rectTransform.sizeDelta);
            }
            
            // CreateDragGhost();
            // UpdateGhostPosition(eventData);
            _owner?.HideTooltip();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // UpdateGhostPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.LogWarning("OnEndDrag called");
            ToolDragVisualController.Instance?.Hide();
            // DestroyDragGhost();
            ToolDragPayload.Clear();
        }
        
        private void TryRemoveFromLoadoutByDoubleClick()
        {
            if (PreparationManager.Instance == null) return;

            PreparationLoadout loadout = PreparationManager.Instance.CurrentLoadout;
            if (loadout == null) return;
            
            int slotIndex = loadout.GetSlotIndexOfTool(_tool);
            if (slotIndex < 0) return;

            bool removed = PreparationManager.Instance.RemoveToolFromSlot(slotIndex);
            if (!removed) return;

            if (_owner != null) _owner.ClearSelectedToolIfMatches(_tool);
        }
        
        // private void CreateDragGhost()
        // {
        //     if (_tool == null) return;
        //     
        //     _dragGhostObject = new GameObject("ToolDragGhost");
        //     _dragGhostObject.transform.SetParent(_canvasRect, false);
        //     
        //     _dragGhostRect = _dragGhostObject.AddComponent<RectTransform>();
        //     _dragGhostImage = _dragGhostObject.AddComponent<Image>();
        //     
        //     _dragGhostImage.raycastTarget = false;
        //     _dragGhostImage.sprite = _tool.Icon;
        //     
        //     if (iconImage != null) _dragGhostRect.sizeDelta = iconImage.rectTransform.sizeDelta;
        // }
        //
        // private void DestroyDragGhost()
        // {
        //     if (_dragGhostObject != null)
        //     {
        //         _dragGhostObject.SetActive(false);
        //         Destroy(_dragGhostObject);
        //     }
        //     
        //     _dragGhostObject = null;
        //     _dragGhostRect = null;
        //     _dragGhostImage = null;
        // }
        //
        // private void UpdateGhostPosition(PointerEventData eventData)
        // {
        //     if (_dragGhostRect == null || _canvasRect == null || _canvas == null) return;
        //     
        //     UnityEngine.Camera uiCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
        //
        //     if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, eventData.position, uiCamera,
        //             out Vector2 localPoint))
        //     {
        //         _dragGhostRect.anchoredPosition = localPoint;
        //     }
        // }
    }
}
