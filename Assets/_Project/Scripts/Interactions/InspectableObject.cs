using _Project.Scripts.Core.Data.Investigation;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class InspectableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private ClueFragmentDefinition clueFragment;
        [SerializeField] private bool canInteractOnlyOnce = true;
        
        private bool _hasBeenInspected;

        public void Interact()
        {
            if (canInteractOnlyOnce && _hasBeenInspected) return;
            if (clueFragment == null) return;
            if (InvestigationManager.Instance == null) return;
            
            InvestigationManager.Instance.AddClue(clueFragment);
            Debug.Log($"clue added: {clueFragment.DisplayName}");
            _hasBeenInspected = true;
        }
    }
}
