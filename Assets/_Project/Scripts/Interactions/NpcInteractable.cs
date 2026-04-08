using _Project.Scripts.Core.Data.Guide;
using _Project.Scripts.Core.Data.NPC;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class NpcInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private NpcDefinition npcDefinition;
        [SerializeField] private GuideObjectiveReporter objectiveReporter;

        public void Interact()
        {
            if (npcDefinition == null) return;
            
            Debug.Log($"Interacted with NPC: {npcDefinition.NpcID}");

            HandleDialogFinished();
        }

        private void HandleDialogFinished()
        {
            if (objectiveReporter != null) objectiveReporter.ReportObjective();
        }
    }
}
