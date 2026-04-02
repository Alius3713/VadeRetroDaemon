using System.Collections.Generic;
using _Project.Scripts.Core.Data.Demons;
using _Project.Scripts.Core.Data.Investigation;
using _Project.Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Demonarium
{
    public class DemonariumBookUI : MonoBehaviour
    {
        [Header("Right Page")]
        [SerializeField] private TextMeshProUGUI demonNameText;
        [SerializeField] private TextMeshProUGUI demonDescriptionText;
        [SerializeField] private TextMeshProUGUI excludedText;
        
        [Header("Navigation")]
        [SerializeField] private Button previousPageButton;
        [SerializeField] private Button nextPageButton;

        private readonly List<DemonDefinition> _unlockedDemons = new();
        private int _currentPageIndex;

        private void Awake()
        {
            if (excludedText != null) excludedText.gameObject.SetActive(false);

            if (previousPageButton != null)
            {
                previousPageButton.onClick.RemoveAllListeners();
                previousPageButton.onClick.AddListener(ShowPreviousPage);
            }

            if (nextPageButton != null)
            {
                nextPageButton.onClick.RemoveAllListeners();
                nextPageButton.onClick.AddListener(ShowNextPage);
            }
        }

        private void OnEnable()
        {
            SubscribeToManagersEvents();
            RefreshView();
        }

        private void OnDisable()
        {
            UnsubscribeFromManagersEvents();
        }
        
        public void RefreshView()
        {
            RefreshUnlockedDemons();
            RefreshCurrentPage();
        }
        
        private void SubscribeToManagersEvents()
        {
            if (DemonariumManager.Instance != null)
            {
                DemonariumManager.Instance.OnDemonUnlocked += HandleDemonUnlocked;
                DemonariumManager.Instance.OnDemonariumInitialized += HandleDemonariumChanged;
                DemonariumManager.Instance.OnDemonariumCleared += HandleDemonariumChanged;
            }

            if (InvestigationManager.Instance != null)
            {
                InvestigationManager.Instance.OnNotebookEntryAdded += HandleNotebookChanged;
                InvestigationManager.Instance.OnNotebookEntryUpdated += HandleNotebookChanged;
                InvestigationManager.Instance.OnNotebookCleared += HandleNotebookCleared;
            }
        }

        private void UnsubscribeFromManagersEvents()
        {
            if (DemonariumManager.Instance != null)
            {
                DemonariumManager.Instance.OnDemonUnlocked -= HandleDemonUnlocked;
                DemonariumManager.Instance.OnDemonariumInitialized -= HandleDemonariumChanged;
                DemonariumManager.Instance.OnDemonariumCleared -= HandleDemonariumChanged;
            }

            if (InvestigationManager.Instance != null)
            {
                InvestigationManager.Instance.OnNotebookEntryAdded -= HandleNotebookChanged;
                InvestigationManager.Instance.OnNotebookEntryUpdated -= HandleNotebookChanged;
                InvestigationManager.Instance.OnNotebookCleared -= HandleNotebookCleared;
            }
        }
        
        public void ShowPreviousPage()
        {
            if (_unlockedDemons.Count == 0) return;

            _currentPageIndex--;
            if (_currentPageIndex < 0) _currentPageIndex = _unlockedDemons.Count - 1;

            RefreshCurrentPage();
        }

        public void ShowNextPage()
        {
            if (_unlockedDemons.Count == 0) return;
            
            _currentPageIndex++;
            if (_currentPageIndex >= _unlockedDemons.Count) _currentPageIndex = 0;

            RefreshCurrentPage();
        }

        private void RefreshUnlockedDemons()
        {
            _unlockedDemons.Clear();
            
            if (DemonariumManager.Instance == null) return;

            IReadOnlyList<DemonDefinition> demons = DemonariumManager.Instance.GetUnlockedDemons();

            for (int i = 0; i < demons.Count; i++)
            {
                DemonDefinition demon = demons[i];
                if (demon == null) continue;
                
                _unlockedDemons.Add(demon);
            }
            
            if (_currentPageIndex >= _unlockedDemons.Count) _currentPageIndex = Mathf.Max(0, _unlockedDemons.Count - 1);
        }

        private void RefreshCurrentPage()
        {
            Debug.Log("DemonariumBookUI.RefreshCurrentPage called");
            
            if (_unlockedDemons.Count == 0)
            {
                SetEmptyPage();
                return;
            }
            
            if (_currentPageIndex < 0 || _currentPageIndex >= _unlockedDemons.Count) _currentPageIndex = 0;
            
            DemonDefinition currentDemon = _unlockedDemons[_currentPageIndex];
            if (currentDemon == null)
            {
                SetEmptyPage();
                return;
            }

            if (demonNameText != null) demonNameText.text = currentDemon.DisplayName;
            if (demonDescriptionText != null) demonDescriptionText.text = currentDemon.Description;

            RefreshExcludedState(currentDemon);
        }

        private void RefreshExcludedState(DemonDefinition demon)
        {
            if (excludedText == null) return;
            
            bool isExcluded = false;
            if (InvestigationManager.Instance != null)
            {
                InvestigationNotebook notebook = InvestigationManager.Instance.CurrentInvestigationNotebook;
                isExcluded = DeductionEvaluator.IsDemonExcluded(demon, notebook);
            }
            
            excludedText.gameObject.SetActive(isExcluded);
        }

        private void SetEmptyPage()
        {
            if (demonNameText != null) demonNameText.text = string.Empty;
            if (demonDescriptionText != null) demonDescriptionText.text = string.Empty;
            if (excludedText != null) excludedText.gameObject.SetActive(false);
        }

        private void HandleDemonUnlocked(DemonDefinition demon)
        {
            RefreshUnlockedDemons();
            
            if(isActiveAndEnabled) RefreshCurrentPage();
        }

        private void HandleDemonariumChanged()
        {
            RefreshUnlockedDemons();
            
            if (isActiveAndEnabled) RefreshCurrentPage();
        }

        private void HandleNotebookChanged(ClueNotebookEntry entry)
        {
            if (!isActiveAndEnabled) return;
            RefreshCurrentPage();
        }

        private void HandleNotebookCleared()
        {
            if (!isActiveAndEnabled) return;
            RefreshCurrentPage();
        }
    }
}
