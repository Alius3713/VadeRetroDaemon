using _Project.Scripts.Core.Data.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Preparation
{
    public class ToolsCollectionItemUI : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject selectedFrame;
        [SerializeField] private GameObject inLoadoutMarker;

        private ToolDefinition _tool;
        private PreparationPanelUI _owner;
        
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
            
            _owner.HandleCollectionToolClicked(_tool);
        }
    }
}
