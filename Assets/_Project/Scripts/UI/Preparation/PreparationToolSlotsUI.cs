using _Project.Scripts.Core.Data.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Preparation
{
    public class PreparationToolSlotsUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject filledVisual;
        
        private PreparationPanelUI _owner;
        private int _slotIndex;
        private ToolDefinition _boundTool;
        
        private float _lastClickTime;
        private const float DoubleClickThreshold = 0.3f;

        public void Initialize(PreparationPanelUI owner, int slotIndex)
        {
            _owner = owner;
            _slotIndex = slotIndex;
        }

        public void Bind(ToolDefinition tool)
        {
            _boundTool = tool;
            
            bool hasTool = _boundTool != null;

            if (iconImage != null)
            {
                iconImage.sprite = hasTool ? _boundTool.Icon : null;
                iconImage.enabled = hasTool && _boundTool.Icon != null;
            }

            if (filledVisual != null)
            {
                filledVisual.SetActive(hasTool);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_owner == null || _boundTool == null) return;

            if (Time.unscaledTime - _lastClickTime <= DoubleClickThreshold)
            {
                _owner.TryRemoveToolFromSlot(_slotIndex);
                _lastClickTime = 0f;
                return;
            }
            
            _lastClickTime = Time.unscaledTime;
        }
    }
}
