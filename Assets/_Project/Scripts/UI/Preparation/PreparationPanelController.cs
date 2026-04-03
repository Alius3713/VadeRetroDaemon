using System;
using _Project.Scripts.Core;
using _Project.Scripts.UI.Demonarium;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.UI.Preparation
{
    public class PreparationPanelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject preparationPanel;
        [SerializeField] private PreparationPanelUI preparationPanelUI;
        [SerializeField] private DemonariumBookPanelController demonariumBookPanelController;
        
        private InputAction _togglePreparationAction;
        private bool _isOpen;
        
        public bool IsOpen => _isOpen;

        private void Awake()
        {
            _togglePreparationAction = InputSystem.actions.FindAction("TogglePreparation");

            if (_togglePreparationAction == null)
            {
                Debug.LogError($"[PreparationPanelController] TogglePreparation action not found!");
            }

            if (preparationPanel != null)
            {
                preparationPanel.SetActive(false);
            }
            
            _isOpen = false;
        }

        private void OnEnable()
        {
            if (_togglePreparationAction != null)
            {
                _togglePreparationAction.performed += OnTogglePreparationPerformed;
            }
        }

        private void OnDisable()
        {
            if (_togglePreparationAction != null)
            {
                _togglePreparationAction.performed -= OnTogglePreparationPerformed;
            }
        }

        public void TogglePrepWindow()
        {
            if (_isOpen)
            {
                ClosePrepWindow();
            }
            else
            {
                OpenPrepWindow();
            }
        }

        public void OpenPrepWindow()
        {
            if (_isOpen) return;
            if (preparationPanel == null) return;

            if (demonariumBookPanelController != null && demonariumBookPanelController.IsOpen)
            {
                demonariumBookPanelController.CloseBook();
            }
            
            preparationPanel.SetActive(true);
            preparationPanelUI?.Refresh();
            
            InputLock.SetOccupied(true);
            _isOpen = true;
        }

        public void ClosePrepWindow()
        {
            if (!_isOpen) return;
            if (preparationPanel == null) return;
            
            preparationPanel.SetActive(false);
            
            InputLock.SetOccupied(false);
            _isOpen = false;
        }

        private void OnTogglePreparationPerformed(InputAction.CallbackContext context)
        {
            TogglePrepWindow();
        }
    }
}
