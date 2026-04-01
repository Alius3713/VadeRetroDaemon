using System;
using System.Linq;
using _Project.Scripts.Core.Data.Demons;
using _Project.Scripts.Core.Data.Investigation;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class InvestigationManager : MonoBehaviour
    {
        public static InvestigationManager Instance { get; private set; }
        
        private InvestigationState _currentInvestigationState;
        private InvestigationNotebook _currentInvestigationNotebook;
        
        public InvestigationState CurrentInvestigationState  => _currentInvestigationState;
        public InvestigationNotebook CurrentInvestigationNotebook => _currentInvestigationNotebook;

        public event Action<ClueNotebookEntry> OnNotebookEntryAdded;
        public event Action<ClueNotebookEntry> OnNotebookEntryUpdated;
        public event Action OnNotebookCleared;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _currentInvestigationState = new InvestigationState();
            _currentInvestigationNotebook = new InvestigationNotebook();
        }

        public void AddClue(ClueFragmentDefinition clueFragment)
        {
            if (clueFragment == null) return;
            if (_currentInvestigationState == null) return;
            if (_currentInvestigationNotebook == null) return;
            
            bool alreadtDiscovered = _currentInvestigationState.DiscoveredClues.Contains(clueFragment);
            if (alreadtDiscovered) return;
            
            _currentInvestigationState.AddClue(clueFragment);

            ClueNotebookEntry newEntry = new ClueNotebookEntry
            {
                SourceClue = clueFragment,
                AssignedTait = null,
                EvaluationState = ClueEvaluationState.Unsure
            };
            
            _currentInvestigationNotebook.AddEntry(newEntry);
            OnNotebookEntryAdded?.Invoke(newEntry);
        }

        public ClueNotebookEntry GetNotebookEntryForClue(ClueFragmentDefinition clueFragment)
        {
            if (_currentInvestigationNotebook == null) return null;
            
            return _currentInvestigationNotebook.GetEntryForClue(clueFragment);
        }

        public void SetNotebookEntryTrait(ClueFragmentDefinition clueFragment, TraitDefinition trait)
        {
            ClueNotebookEntry entry = GetNotebookEntryForClue(clueFragment);
            if (entry == null) return;
            
            entry.AssignedTait = trait;
            OnNotebookEntryUpdated?.Invoke(entry);
        }

        public void SetNotebookEntryEvaluation(ClueFragmentDefinition clueFragment, ClueEvaluationState evaluationState)
        {
            ClueNotebookEntry entry = GetNotebookEntryForClue(clueFragment);
            if (entry == null) return;
            
            entry.EvaluationState = evaluationState;
            OnNotebookEntryUpdated?.Invoke(entry);
        }

        public void ClearInvestigation()
        {
            _currentInvestigationState = new InvestigationState();
            _currentInvestigationNotebook = new InvestigationNotebook();
            OnNotebookCleared?.Invoke();
        }
    }
}
