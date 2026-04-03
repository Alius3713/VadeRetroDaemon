using _Project.Scripts.Core.Data.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.UI.Preparation
{
    public class ToolTooltipUI : MonoBehaviour
    {
        [SerializeField] private RectTransform panelRoot;
        [SerializeField] private TextMeshProUGUI toolNameText;
        [SerializeField] private TextMeshProUGUI toolDescriptionText;
        [SerializeField] private Vector2 screenOffset = new Vector2(16f, -16f);
        
        private Canvas _canvas;
        private UnityEngine.Camera _uiCamera;
        private bool _isVisible;
        
        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();

            if (_canvas != null && _canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                _uiCamera = _canvas.worldCamera;
            }

            Hide();
        }

        private void Update()
        {
            if (!_isVisible) return;
            if (Mouse.current == null) return;
            
            SetPosition(Mouse.current.position.ReadValue());
        }

        public void Show(ToolDefinition tool, Vector2 screenPosition)
        {
            if (tool == null) return;
            if (toolNameText != null)
            {
                toolNameText.text = tool.DisplayName;
            }

            if (toolDescriptionText != null)
            {
                toolDescriptionText.text = tool.Description;
            }

            if (panelRoot != null)
            {
                panelRoot.gameObject.SetActive(true);
            }
            
            _isVisible = true;
            SetPosition(screenPosition);
        }

        public void Hide()
        {
            if (panelRoot != null)
            {
                panelRoot.gameObject.SetActive(false);
            }
            
            _isVisible = false;
        }

        private void SetPosition(Vector2 screenPosition)
        {
            if (!panelRoot || !_canvas) return;
            
            RectTransform canvasRect = _canvas.transform as RectTransform;
            if (!canvasRect) return;

            Vector2 anchoredPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPosition + screenOffset,
                _uiCamera,
                out anchoredPosition
            );
            
            panelRoot.anchoredPosition = anchoredPosition;
        }
    }
}
