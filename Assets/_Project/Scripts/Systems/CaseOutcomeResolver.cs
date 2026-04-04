using _Project.Scripts.Core.Data.Cases;
using _Project.Scripts.Core.Enums;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class CaseOutcomeResolver : MonoBehaviour
    {
        public void ResolveOutcome(CaseDefinition caseDef, ResolutionOutcome outcome)
        {
            if (caseDef == null) return;

            switch (outcome)
            {
                case ResolutionOutcome.Success:
                    Debug.Log($"CASE RESULT: {caseDef.DisplayName} -> Success");
                    break;
                
                case ResolutionOutcome.PartialSuccess:
                    Debug.Log($"CASE RESULT: {caseDef.DisplayName} -> Partial Success");
                    break;
                
                case ResolutionOutcome.PartialFailure:
                    Debug.Log($"CASE RESULT: {caseDef.DisplayName} -> Partial Failure");
                    break;
                
                case ResolutionOutcome.Failure:
                    Debug.Log($"CASE RESULT: {caseDef.DisplayName} -> Failure");
                    break;
            }
            
        }
    }
}
