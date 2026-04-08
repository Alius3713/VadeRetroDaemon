using _Project.Scripts.Core;
using _Project.Scripts.Dialogue;
using _Project.Scripts.UI.Preparation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.UI.Demonarium
{
    public class DemonariumBookPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject demonariumBookPanel;
        [SerializeField] private DemonariumBookUI demonariumBookUI;
        [SerializeField] private PreparationPanelController preparationPanelController;
        
        private InputAction _toggleDemonariumAction;
        private InputAction _cancelAction;
        
        private bool _isOpen;
        
        public bool IsOpen => _isOpen;
        
        private void Awake()
        {
            _toggleDemonariumAction = InputSystem.actions.FindAction("ToggleDemonarium");
            _cancelAction = InputSystem.actions.FindAction("Cancel");

            if (_toggleDemonariumAction == null)
            {
                Debug.LogError("ToggleDemonarium action not found.");
            }

            if (demonariumBookPanel != null)
            {
                demonariumBookPanel.SetActive(false);
            }

            _isOpen = false;
        }
        
        private void OnEnable()
        {
            if (_toggleDemonariumAction != null)
            {
                _toggleDemonariumAction.performed += OnToggleDemonariumPerformed;
            }

            if (_cancelAction != null) _cancelAction.performed += HandleCancel;
        }

        private void OnDisable()
        {
            if (_toggleDemonariumAction != null)
            {
                _toggleDemonariumAction.performed -= OnToggleDemonariumPerformed;
            }

            if (_cancelAction != null) _cancelAction.performed -= HandleCancel;
        }
        
        public void ToggleBook()
        {
            if (WindowsInputLock.Occupied) return;
            if (GameDialogService.Instance != null && GameDialogService.Instance.IsDialogPlaying) return;
            
            if (_isOpen)
            {
                CloseBook();
            }
            else
            {
                OpenBook();
            }
        }

        public void OpenBook()
        {
            if (WindowsInputLock.Occupied) return;
            if (GameDialogService.Instance != null && GameDialogService.Instance.IsDialogPlaying) return;
            
            if (_isOpen) return;
            if (demonariumBookPanel == null) return;

            if (preparationPanelController != null && preparationPanelController.IsOpen)
            {
                preparationPanelController.ClosePrepWindow();
            }
            
            demonariumBookPanel.SetActive(true);
            InputLock.SetOccupied(true);
            _isOpen = true;

            demonariumBookUI?.RefreshView();
        }

        public void CloseBook()
        {
            if (!_isOpen) return;
            if (demonariumBookPanel == null) return;

            demonariumBookPanel.SetActive(false);
            InputLock.SetOccupied(false);
            _isOpen = false;
        }

        private void OnToggleDemonariumPerformed(InputAction.CallbackContext ctx)
        {
            if (WindowsInputLock.Occupied) return;
            ToggleBook();
        }

        private void HandleCancel(InputAction.CallbackContext ctx)
        {
            if (WindowsInputLock.Occupied) return;
            if (!_isOpen) return;
            CloseBook();
        }
    }
}
