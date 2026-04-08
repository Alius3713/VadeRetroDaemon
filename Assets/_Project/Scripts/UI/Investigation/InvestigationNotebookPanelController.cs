using _Project.Scripts.Core;
using _Project.Scripts.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.UI.Investigation
{
    public class InvestigationNotebookPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject notebookPanel;
        [SerializeField] private InvestigationNotebookUI notebookUI;
        
        private InputAction _toggleNotebookAction;
        private InputAction _cancelAction;
        
        private bool _isOpen;

        private void Awake()
        {
            _toggleNotebookAction = InputSystem.actions.FindAction("ToggleNotebook");
            _cancelAction = InputSystem.actions.FindAction("Cancel");
            
            if (notebookUI == null) notebookUI = FindFirstObjectByType<InvestigationNotebookUI>();
            
            if (_toggleNotebookAction == null)
            {
                Debug.LogError("ToggleNotebook action not found");
            }
            
            if (notebookUI != null) notebookPanel.SetActive(false);
            _isOpen = false;
        }

        private void OnEnable()
        {
            if (_toggleNotebookAction != null)
            {
                _toggleNotebookAction.performed += OnToggleNotebookPerformed;
            }
            
            if (_cancelAction != null) _cancelAction.performed += HandleCancel;
        }

        private void OnDestroy()
        {
            if (_toggleNotebookAction != null)
            {
                _toggleNotebookAction.performed -= OnToggleNotebookPerformed;
            }

            if (_cancelAction != null) _cancelAction.performed -= HandleCancel;
        }

        public void ToggleNotebook()
        {
            if (WindowsInputLock.Occupied) return;
            if (GameDialogService.Instance != null && GameDialogService.Instance.IsDialogPlaying) return;
            
            if (_isOpen)
            {
                CloseNotebook();
            }
            else
            {
                OpenNotebook();
            }
        }

        public void OpenNotebook()
        {
            if (WindowsInputLock.Occupied) return;
            if (GameDialogService.Instance != null && GameDialogService.Instance.IsDialogPlaying) return;
            
            if (_isOpen) return;
            if (notebookPanel == null) return;
            
            notebookPanel.SetActive(true);
            notebookUI?.Refresh();
            _isOpen = true;
        }

        public void CloseNotebook()
        {
            if (!_isOpen) return;
            if (notebookPanel == null) return;
            
            notebookPanel.SetActive(false);
            _isOpen = false;
        }
        
        private void OnToggleNotebookPerformed(InputAction.CallbackContext obj)
        {
            if (WindowsInputLock.Occupied) return;
            ToggleNotebook();
        }

        private void HandleCancel(InputAction.CallbackContext ctx)
        {
            if (WindowsInputLock.Occupied) return;
            if (!_isOpen) return;
            CloseNotebook();
        }
    }
}
