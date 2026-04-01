using System;
using _Project.Scripts.Core.Data.Investigation;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class InvestigationManager : MonoBehaviour
    {
        public static InvestigationManager Instance { get; private set; }
        
        private InvestigationState _currentInvestigationState;
        
        public InvestigationState CurrentInvestigationState  => _currentInvestigationState;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _currentInvestigationState = new InvestigationState();
        }

        public void AddClue(ClueFragmentDefinition clueFragment)
        {
            if (clueFragment == null) return;
            
            _currentInvestigationState.AddClue(clueFragment);
        }
    }
}
