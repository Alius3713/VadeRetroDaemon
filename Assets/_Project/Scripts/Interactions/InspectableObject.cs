using System;
using _Project.Scripts.Core.Data.Investigation;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class InspectableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private InvestigationTarget investigationTarget;
        [SerializeField] private bool canInteractOnlyOnce = true;
        
        private bool _hasBeenInspected;

        private void Awake()
        {
            if (investigationTarget == null) investigationTarget = GetComponent<InvestigationTarget>();
        }

        public void Interact()
        {
            if (canInteractOnlyOnce && _hasBeenInspected) return;
            if (investigationTarget == null) return;
            if (investigationTarget.ClueFragment == null) return;
            if (InvestigationManager.Instance == null) return;
            
            InvestigationManager.Instance.AddClue(investigationTarget.ClueFragment);
            Debug.Log($"clue added: {investigationTarget.ClueFragment.DisplayName}");
            _hasBeenInspected = true;
        }
    }
}
