using _Project.Scripts.Core.Data.Guide;
using _Project.Scripts.Core.Data.Investigation;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class InspectableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private InvestigationTarget investigationTarget;
        [SerializeField] private GuideObjectiveReporter objectiveReporter;
        [SerializeField] private bool canInteractOnlyOnce = true;
        
        private bool _hasBeenInspected;

        private void Awake()
        {
            if (investigationTarget == null) investigationTarget = GetComponent<InvestigationTarget>();
            if (objectiveReporter == null) objectiveReporter = GetComponent<GuideObjectiveReporter>();
        }

        public void Interact()
        {
            if (canInteractOnlyOnce && _hasBeenInspected) return;
            if (investigationTarget == null) return;
            if (investigationTarget.ClueFragment == null) return;
            if (InvestigationManager.Instance == null) return;
            
            InvestigationManager.Instance.AddClue(investigationTarget.ClueFragment);
            if (objectiveReporter != null)
            {
                objectiveReporter.ReportObjective();
                Debug.Log($"Reported {objectiveReporter.ObjectiveID}");
            }
            _hasBeenInspected = true;
            Debug.Log($"clue added: {investigationTarget.ClueFragment.DisplayName}");
        }
    }
}
