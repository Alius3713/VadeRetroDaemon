using System;
using _Project.Scripts.Core.Data.Cases;
using _Project.Scripts.Core.Data.Demons;
using _Project.Scripts.Core.Data.Preparation;
using _Project.Scripts.Core.Enums;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class CaseRuntimeManager : MonoBehaviour
    {
        public static CaseRuntimeManager Instance  { get; private set; }
        
        [Header("References")]
        [SerializeField] private CaseOutcomeResolver caseOutcomeResolver;
        
        [Header("Debug / Testing")]
        [SerializeField] private CaseDefinition tutorialCase;
        
        private CaseDefinition _currentCase;
        
        public CaseDefinition CurrentCase => _currentCase;
        public bool HasActiveCase => _currentCase != null;

        public event Action<CaseDefinition> OnCaseStarted;
        public event Action<CaseDefinition, ResolutionOutcome> OnCaseResolved;
        public event Action OnCaseCleared;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            if (caseOutcomeResolver == null) caseOutcomeResolver = GetComponent<CaseOutcomeResolver>();
        }

        private void Start()
        {
            if (PreparationManager.Instance != null)
            {
                PreparationManager.Instance.OnPreparationConfirmed += HandlePreparationConfirmed;
                Debug.Log("CaseRuntimeManager subscribed to PreparationManager.OnPreparationConfirmed");
            }
            else
            {
                Debug.LogError("PreparationManager.Instance is null in CaseRuntimeManager.Start");
            }
            
            if (tutorialCase != null) StartCase(tutorialCase);
        }
        
        private void OnDestroy()
        {
            if (PreparationManager.Instance != null)
            {
                PreparationManager.Instance.OnPreparationConfirmed -= HandlePreparationConfirmed;
            }
        }
        
        public void StartCase(CaseDefinition caseDef)
        {
            if (caseDef == null) return;
            
            _currentCase = caseDef;
            OnCaseStarted?.Invoke(_currentCase);
            
            Debug.Log($"CASE STARTED: {_currentCase.DisplayName}");
        }
        
        public void ClearCurrentCase()
        {
            _currentCase = null;
            OnCaseCleared?.Invoke();
        }

        private void HandlePreparationConfirmed(PreparationLoadout loadout)
        {
            if (_currentCase == null || loadout == null) return;
            
            ResolutionMethodDefinition selectedMethod = loadout.SelectedMethod;
            if (selectedMethod == null) return;

            DemonDefinition demon = _currentCase.Demon;
            if (demon == null) return;
            
            ResolutionOutcome outcome = ResultEvaluator.Evaluate(
                demon.Traits,
                selectedMethod.SupportedResolutionTraits);
            
            caseOutcomeResolver?.ResolveOutcome(_currentCase, outcome);
            OnCaseResolved?.Invoke(_currentCase, outcome);
            
            ResetCaseRuntime();
        }

        private void ResetCaseRuntime()
        {
            if (InvestigationManager.Instance != null) InvestigationManager.Instance.ClearInvestigation();
            if (PreparationManager.Instance != null) PreparationManager.Instance.ClearAndReinitialize();
            
            ClearCurrentCase();
        }
    }
}
