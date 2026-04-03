using _Project.Scripts.Core;
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
        private bool _isOpen;
        
        public bool IsOpen => _isOpen;
        
        private void Awake()
        {
            _toggleDemonariumAction = InputSystem.actions.FindAction("ToggleDemonarium");

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
        }

        private void OnDisable()
        {
            if (_toggleDemonariumAction != null)
            {
                _toggleDemonariumAction.performed -= OnToggleDemonariumPerformed;
            }
        }
        
        public void ToggleBook()
        {
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
            ToggleBook();
        }
    }
}
