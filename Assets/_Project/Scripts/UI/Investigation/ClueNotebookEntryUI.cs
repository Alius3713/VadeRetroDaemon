using System.Collections.Generic;
using _Project.Scripts.Core.Data.Demons;
using _Project.Scripts.Core.Data.Investigation;
using _Project.Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Investigation
{
    public class ClueNotebookEntryUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI clueNameText;
        [SerializeField] private TMP_Dropdown traitsDropdown;
        [SerializeField] private Button unsureButton;
        [SerializeField] private Button legitButton;
        [SerializeField] private Button falseLeadButton;
        [SerializeField] private TextMeshProUGUI evaluationText;
        
        private ClueNotebookEntry _entry;
        private List<TraitDefinition> _availableTraits;
        private InvestigationNotebookUI _owner;

        public void Initialize(ClueNotebookEntry entry, List<TraitDefinition> availableTraits,
            InvestigationNotebookUI owner)
        {
            _entry = entry;
            _availableTraits = availableTraits;
            _owner = owner;

            SetupClueLabel();
            SetupTraitsDropdown();
            SetupButtons();
            RefreshEvaluationLabel();
        }

        private void SetupClueLabel()
        {
            if (clueNameText == null) return;

            clueNameText.text = _entry != null && _entry.SourceClue != null
                ? _entry.SourceClue.Description
                : "Missing clue";
        }

        private void SetupTraitsDropdown()
        {
            if (traitsDropdown == null) return;
            
            traitsDropdown.onValueChanged.RemoveAllListeners();
            traitsDropdown.ClearOptions();

            List<string> options = new() { "No Trait" };

            for (int i = 0; i < _availableTraits.Count; i++)
            {
                TraitDefinition trait = _availableTraits[i];
                options.Add(trait != null ? trait.DisplayName : "Missing trait");
            }
            traitsDropdown.AddOptions(options);

            int currentIndex = 0;

            if (_entry != null && _entry.AssignedTait != null)
            {
                for (int i = 0; i < _availableTraits.Count; i++)
                {
                    if (_availableTraits[i] == _entry.AssignedTait)
                    {
                        currentIndex = i + 1;
                        break;
                    }
                }
            }
            
            traitsDropdown.SetValueWithoutNotify(currentIndex);
            traitsDropdown.onValueChanged.AddListener(OnTraitDropdownChanged);
        }

        private void SetupButtons()
        {
            if (unsureButton != null)
            {
                unsureButton.onClick.RemoveAllListeners();
                unsureButton.onClick.AddListener(SetUnsure);
            }

            if (legitButton != null)
            {
                legitButton.onClick.RemoveAllListeners();
                legitButton.onClick.AddListener(SetLegit);
            }

            if (falseLeadButton != null)
            {
                falseLeadButton.onClick.RemoveAllListeners();
                falseLeadButton.onClick.AddListener(SetFalseLead);
            }
        }

        private void OnTraitDropdownChanged(int dropdownIndex)
        {
            if (_entry == null || _entry.SourceClue == null) return;
            if (InvestigationManager.Instance == null) return;

            TraitDefinition selectedTrait = null;

            if (dropdownIndex > 0)
            {
                int traitIndex = dropdownIndex - 1;
                if (traitIndex < _availableTraits.Count)
                {
                    selectedTrait = _availableTraits[traitIndex];
                }
            }
            
            InvestigationManager.Instance.SetNotebookEntryTrait(_entry.SourceClue, selectedTrait);
        }

        private void SetUnsure()
        {
            SetEvaluationState(ClueEvaluationState.Unsure);
        }

        private void SetLegit()
        {
            SetEvaluationState(ClueEvaluationState.Legit);
        }

        private void SetFalseLead()
        {
            SetEvaluationState(ClueEvaluationState.FalseLead);
        }

        private void SetEvaluationState(ClueEvaluationState state)
        {
            if (_entry == null || _entry.SourceClue == null) return;
            if (InvestigationManager.Instance == null) return;
            
            InvestigationManager.Instance.SetNotebookEntryEvaluation(_entry.SourceClue, state);
        }

        private void RefreshEvaluationLabel()
        {
            if (evaluationText == null || _entry == null) return;

            evaluationText.text = _entry.EvaluationState switch
            {
                ClueEvaluationState.Legit => "Legit",
                ClueEvaluationState.FalseLead => "False Lead",
                _ => "Unsure"
            };
        }
    }
}
