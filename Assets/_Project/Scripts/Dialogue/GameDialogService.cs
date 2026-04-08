using System;
using _Project.Scripts.Core;
using _Project.Scripts.Systems;
using DialogSystem.Runtime.Core;
using UnityEngine;

namespace _Project.Scripts.Dialogue
{
    public class GameDialogService : MonoBehaviour, IDialogService
    {
        public static GameDialogService Instance { get; private set; }
        
        [Header("Optional UI References")]
        [SerializeField] private GameObject guideUIPanelRoot;
        [SerializeField] private GameObject notebookUIPanelRoot;

        private bool _isDialogPlaying;
        
        private bool _wasGuideUIVisibleBeforeDialog;
        private bool _wasNotebookUIVisibleBeforeDialog;
        
        public bool IsDialogPlaying => _isDialogPlaying;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void PlayDialog(string dialogId, Action onDialogFinished = null)
        {
            if (string.IsNullOrWhiteSpace(dialogId))
            {
                Debug.LogWarning("GameDialogService: dialogId is null or empty.");
                onDialogFinished?.Invoke();
                return;
            }

            if (_isDialogPlaying)
            {
                Debug.LogWarning($"GameDialogService: tried to start dialog '{dialogId}' while another dialog is already playing.");
                return;
            }
            
            if (DialogManager.Instance == null)
            {
                Debug.LogError("GameDialogService: DialogManager.Instance is null.");
                onDialogFinished?.Invoke();
                return;
            }
            
            _isDialogPlaying = true;
            InputLock.SetOccupied(true);
            
            CacheOptionalUIVisibility();
            HideOptionalUIThatWasVisible();
            
            DialogManager.Instance.PlayDialogByID(dialogId, onDialogEnded:() => HandleDialogEnded(onDialogFinished));
        }

        private void HandleDialogEnded(Action onDialogFinished)
        {
            _isDialogPlaying = false;
            InputLock.SetOccupied(false);
            
            onDialogFinished?.Invoke();
            
            RestoreOptionalUIVisibility();
        }
        
        private void CacheOptionalUIVisibility()
        {
            _wasGuideUIVisibleBeforeDialog = guideUIPanelRoot != null && guideUIPanelRoot.activeSelf;
            _wasNotebookUIVisibleBeforeDialog = notebookUIPanelRoot != null && notebookUIPanelRoot.activeSelf;
        }

        private void HideOptionalUIThatWasVisible()
        {
            if (_wasGuideUIVisibleBeforeDialog && guideUIPanelRoot != null)
            {
                guideUIPanelRoot.SetActive(false);
            }

            if (_wasNotebookUIVisibleBeforeDialog && notebookUIPanelRoot != null)
            {
                notebookUIPanelRoot.SetActive(false);
            }
        }

        private void RestoreOptionalUIVisibility()
        {
            if (_wasGuideUIVisibleBeforeDialog && guideUIPanelRoot != null && GuideManager.Instance != null && GuideManager.Instance.HasActiveGuide)
            {
                guideUIPanelRoot.SetActive(true);
            }

            if (_wasNotebookUIVisibleBeforeDialog && notebookUIPanelRoot != null)
            {
                notebookUIPanelRoot.SetActive(true);
            }

            _wasGuideUIVisibleBeforeDialog = false;
            _wasNotebookUIVisibleBeforeDialog = false;
        }
    }
}
