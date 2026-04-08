using System.Collections.Generic;
using _Project.Scripts.Core.Data.Guide;
using _Project.Scripts.Core.Data.NPC;
using _Project.Scripts.Dialogue;
using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class NpcInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private NpcDefinition npcDefinition;

        private int _currentDialogueStepIndex;

        public void Interact()
        {
            if (npcDefinition == null) return;
            if (GameDialogService.Instance == null)
            {
                Debug.LogWarning("NpcInteractable: GameDialogService.Instance is null.");
                return;
            }

            NpcDialogueStep step = GetCurrentStep();
            if (step == null)
            {
                Debug.LogWarning($"NpcInteractable: no valid dialogue step for NPC '{npcDefinition.DisplayName}'.");
                return;
            }

            if (string.IsNullOrWhiteSpace(step.DialogId))
            {
                Debug.LogWarning($"NpcInteractable: empty dialogId on current step for NPC '{npcDefinition.DisplayName}'.");
                return;
            }
            
            Debug.Log($"Interacted with NPC: {npcDefinition.NpcID}");

            GameDialogService.Instance.PlayDialog(step.DialogId, HandleDialogFinished);
        }

        private void HandleDialogFinished()
        {
            NpcDialogueStep step = GetCurrentStep();
            if (step == null) return;
            
            TryAdvanceDialogueStep(step);
        }

        private void TryAdvanceDialogueStep(NpcDialogueStep step)
        {
            if (step == null) return;
            
            bool conditionMet = IsStepAdvanceConditionMet(step);

            if (step.LoopUntilConditionMet)
            {
                if (conditionMet) AdvanceToNextStep();
                
                return;
            }

            if (step.PlayOnlyOnce) AdvanceToNextStep();
        }

        private bool IsStepAdvanceConditionMet(NpcDialogueStep step)
        {
            if (step == null) return false;
            if (string.IsNullOrWhiteSpace(step.RequiredObjectiveIdToAdvance)) return true;
            if (!GuideManager.Instance) return false;
            
            return !GuideManager.Instance.IsCurrentObjective(step.RequiredObjectiveIdToAdvance);
        }

        private void AdvanceToNextStep()
        {
            int nextIndex = _currentDialogueStepIndex + 1;
            
            if (!npcDefinition) return;
            if (nextIndex >= npcDefinition.DialogueSteps.Count) return;
            
            _currentDialogueStepIndex = nextIndex;
        }

        private NpcDialogueStep GetCurrentStep()
        {
            if (!npcDefinition) return null;
            if (npcDefinition.DialogueSteps == null) return null;
            if (npcDefinition.DialogueSteps.Count == 0) return null;

            if (_currentDialogueStepIndex < 0 || _currentDialogueStepIndex >= npcDefinition.DialogueSteps.Count)
            {
                _currentDialogueStepIndex = npcDefinition.DialogueSteps.Count - 1;
            }
            
            return npcDefinition.DialogueSteps[_currentDialogueStepIndex];
        }
    }
}
