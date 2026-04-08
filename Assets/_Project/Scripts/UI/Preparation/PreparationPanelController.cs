using _Project.Scripts.Core;
using _Project.Scripts.Dialogue;
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
        private InputAction _cancelAction;
        
        private bool _isOpen;
        
        public bool IsOpen => _isOpen;

        private void Awake()
        {
            _togglePreparationAction = InputSystem.actions.FindAction("TogglePreparation");
            _cancelAction = InputSystem.actions.FindAction("Cancel");

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

            if (_cancelAction != null) _cancelAction.performed += HandleCancel;
        }

        private void OnDisable()
        {
            if (_togglePreparationAction != null)
            {
                _togglePreparationAction.performed -= OnTogglePreparationPerformed;
            }

            if (_cancelAction != null) _cancelAction.performed -= HandleCancel;
        }

        public void TogglePrepWindow()
        {
            if (WindowsInputLock.Occupied) return;
            if (GameDialogService.Instance != null && GameDialogService.Instance.IsDialogPlaying) return;
            
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
            if (WindowsInputLock.Occupied) return;
            if (GameDialogService.Instance != null && GameDialogService.Instance.IsDialogPlaying) return;
            
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
            if (WindowsInputLock.Occupied) return;
            TogglePrepWindow();
        }

        private void HandleCancel(InputAction.CallbackContext ctx)
        {
            if (WindowsInputLock.Occupied) return;
            if (!_isOpen) return;
            ClosePrepWindow();
        }
    }
}
