using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.UI.Investigation
{
    public class InvestigationNotebookPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject notebookPanel;
        [SerializeField] private InvestigationNotebookUI notebookUI;
        
        private InputAction _toggleNotebookAction;
        private bool _isOpen;

        private void Awake()
        {
            _toggleNotebookAction = InputSystem.actions.FindAction("ToggleNotebook");
            
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
        }

        private void OnDestroy()
        {
            if (_toggleNotebookAction != null)
            {
                _toggleNotebookAction.performed -= OnToggleNotebookPerformed;
            }
        }

        public void ToggleNotebook()
        {
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
            ToggleNotebook();
        }
    }
}
