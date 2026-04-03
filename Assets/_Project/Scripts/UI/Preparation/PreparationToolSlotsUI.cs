using _Project.Scripts.Core.Data.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Preparation
{
    public class PreparationToolSlotsUI : MonoBehaviour, IPointerClickHandler, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject filledVisual;
        
        private PreparationPanelUI _owner;
        private int _slotIndex;
        private ToolDefinition _boundTool;
        
        private float _lastClickTime;
        private const float DoubleClickThreshold = 0.3f;

        private bool _isDraggingOwnTool;

        public void Initialize(PreparationPanelUI owner, int slotIndex)
        {
            _owner = owner;
            _slotIndex = slotIndex;
        }

        public void Bind(ToolDefinition tool)
        {
            _boundTool = tool;
            RefreshBoundVisuals(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_owner == null || _boundTool == null) return;

            if (Time.unscaledTime - _lastClickTime <= DoubleClickThreshold)
            {
                bool removed = _owner.TryRemoveToolFromSlot(_slotIndex);

                if (removed)
                {
                    _boundTool = null;
                    RefreshBoundVisuals(true);
                }
                
                _lastClickTime = 0f;
                return;
            }
            
            _lastClickTime = Time.unscaledTime;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_owner == null) return;
            if (ToolDragPayload.DraggedTool == null) return;
            
            bool placed = _owner.TryPlaceToolIntoSlot(ToolDragPayload.DraggedTool, _slotIndex);
            if (!placed) return;
            
            ToolDragPayload.MarkDroppedOnSlot();
            
            _boundTool = _owner.GetToolInSlot(_slotIndex);

            if (iconImage != null)
            {
                ToolDragVisualController.Instance?.Hide();
            }
            
            RefreshBoundVisuals(true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_boundTool == null) return;
            
            ToolDragPayload.Set(_boundTool, _slotIndex);
            _isDraggingOwnTool = true;

            if (iconImage != null)
            {
                ToolDragVisualController.Instance?.Show(_boundTool, iconImage.rectTransform.sizeDelta);
            }
            // CreateDragGhost(_boundTool);
            
            RefreshBoundVisuals(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // UpdateGhostPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isDraggingOwnTool)
            {
                if (ToolDragPayload.WasDroppedOnSlot)
                {
                    _boundTool = _owner != null ? _owner.GetToolInSlot(_slotIndex) : null;
                    RefreshBoundVisuals(true);
                }
                else
                {
                    bool removed = _owner != null && _owner.TryRemoveToolFromSlot(_slotIndex);
                    if (removed)
                    {
                        _boundTool = null;
                    }
                }
                
                RefreshBoundVisuals(true);
            }

            _isDraggingOwnTool = false;
            ToolDragVisualController.Instance?.Hide();
            // DestroyDragGhost();
            ToolDragPayload.Clear();
        }

        // private void CreateDragGhost(ToolDefinition tool)
        // {
        //     if (tool == null) return;
        //     
        //     _dragGhostObject = new GameObject("ToolDragGhost");
        //     _dragGhostObject.transform.SetParent(_canvasRect, false);
        //     
        //     _dragGhostRect = _dragGhostObject.AddComponent<RectTransform>();
        //     _dragGhostImage = _dragGhostObject.AddComponent<Image>();
        //     
        //     _dragGhostImage.raycastTarget = false;
        //     _dragGhostImage.sprite = tool.Icon;
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

        private void RefreshBoundVisuals(bool visible)
        {
            bool hasTool = _boundTool != null;

            if (iconImage != null)
            {
                iconImage.sprite = hasTool ? _boundTool.Icon : null;
                iconImage.enabled = visible && hasTool && _boundTool.Icon != null;
            }
            
            if (filledVisual != null) filledVisual.SetActive(visible && hasTool);
        }
    }
}
