using System.Collections.Generic;
using _Project.Scripts.Core.Data.Demons;
using _Project.Scripts.Core.Data.Investigation;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.UI.Investigation
{
    public class InvestigationNotebookUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform entriesContainer;
        [SerializeField] private ClueNotebookEntryUI entryPrefab;

        [Header("Data")]
        [SerializeField] private List<TraitDefinition> availableTraits = new();

        private readonly List<ClueNotebookEntryUI> _spawnedEntries = new();

        private void OnEnable()
        {
            SubscribeToManagerEvents();
            Refresh();
        }

        private void OnDisable()
        {
            UnsubscribeFromManagerEvents();
        }

        public void Refresh()
        {
            ClearSpawnedEntries();
            
            if (InvestigationManager.Instance == null) return;
            
            InvestigationNotebook notebook = InvestigationManager.Instance.CurrentInvestigationNotebook;
            if (notebook == null) return;
            
            IReadOnlyList<ClueNotebookEntry> entries = notebook.Entries;

            for (int i = 0; i < entries.Count; i++)
            {
                ClueNotebookEntry entry = entries[i];
                if (entry == null) continue;
                
                ClueNotebookEntryUI entryUI = Instantiate(entryPrefab, entriesContainer);
                entryUI.Initialize(entry, availableTraits, this);
                _spawnedEntries.Add(entryUI);
            }
        }
        
        private void HandleNotebookEntryAdded(ClueNotebookEntry entry)
        {
            if (!isActiveAndEnabled) return;
            Refresh();
        }

        private void HandleNotebookEntryUpdated(ClueNotebookEntry entry)
        {
            if (!isActiveAndEnabled) return;
            Refresh();
        }

        private void HandleNotebookCleared()
        {
            if (!isActiveAndEnabled) return;
            Refresh();
        }
        
        public void RequestRefresh()
        {
            Refresh();
        }
        
        private void SubscribeToManagerEvents()
        {
            if (InvestigationManager.Instance == null) return;

            InvestigationManager.Instance.OnNotebookEntryAdded += HandleNotebookEntryAdded;
            InvestigationManager.Instance.OnNotebookEntryUpdated += HandleNotebookEntryUpdated;
            InvestigationManager.Instance.OnNotebookCleared += HandleNotebookCleared;
        }

        private void UnsubscribeFromManagerEvents()
        {
            if (InvestigationManager.Instance == null) return;

            InvestigationManager.Instance.OnNotebookEntryAdded -= HandleNotebookEntryAdded;
            InvestigationManager.Instance.OnNotebookEntryUpdated -= HandleNotebookEntryUpdated;
            InvestigationManager.Instance.OnNotebookCleared -= HandleNotebookCleared;
        }
        
        private void ClearSpawnedEntries()
        {
            for (int i = 0; i < _spawnedEntries.Count; i++)
            {
                if (_spawnedEntries[i] != null)
                {
                    Destroy(_spawnedEntries[i].gameObject);
                }
            }
            
            _spawnedEntries.Clear();
        }
    }
}
