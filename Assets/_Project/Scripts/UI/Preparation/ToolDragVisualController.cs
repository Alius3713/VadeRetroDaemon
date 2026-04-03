using System;
using _Project.Scripts.Core.Data.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Preparation
{
    public class ToolDragVisualController : MonoBehaviour
    {
        public static ToolDragVisualController Instance { get; private set; }
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image ghostImage;
        
        private RectTransform _ghostRect;
        private bool _isVisible;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            _ghostRect = ghostImage.rectTransform;
            Hide();
        }

        private void Update()
        {
            if (!_isVisible) return;
            if (Mouse.current == null) return;
            
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            _ghostRect.position = screenPosition;
        }

        public void Show(ToolDefinition tool, Vector2 size)
        {
            if (tool == null || tool.Icon == null) return;
            
            ghostImage.sprite = tool.Icon;
            ghostImage.rectTransform.sizeDelta = size;
            ghostImage.raycastTarget = false;
            ghostImage.gameObject.SetActive(true);
            _isVisible = true;
        }

        public void Hide()
        {
            ghostImage.sprite = null;
            ghostImage.gameObject.SetActive(false);
            _isVisible = false;
        }
    }
}
