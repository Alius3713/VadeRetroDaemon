using System.Collections.Generic;
using _Project.Scripts.Core.Data.Demons;
using _Project.Scripts.Core.Enums;

namespace _Project.Scripts.Systems
{
    public static class ResultEvaluator
    {
        public static ResolutionOutcome Evaluate(
            IReadOnlyList<TraitDefinition> demonTraits,
            IReadOnlyList<TraitDefinition> methodSupportedResolutionTraits)
        {
            if (demonTraits == null || demonTraits.Count == 0) return ResolutionOutcome.Failure;

            if (methodSupportedResolutionTraits == null || methodSupportedResolutionTraits.Count == 0)
                return ResolutionOutcome.Failure;

            List<TraitDefinition> demonResolutionTraits = new();

            for (int i = 0; i < demonTraits.Count; i++)
            {
                TraitDefinition trait = demonTraits[i];
                if (trait == null) continue;
                if (trait.Category != TraitCategory.Resolution) continue;
                
                demonResolutionTraits.Add(trait);
            }

            if (demonResolutionTraits.Count == 0) return ResolutionOutcome.Failure;
            
            int matchedCount = 0;
            int extraCount = 0;

            for (int i = 0; i < methodSupportedResolutionTraits.Count; i++)
            {
                TraitDefinition methodTrait = methodSupportedResolutionTraits[i];
                if (methodTrait == null) continue;

                if (demonResolutionTraits.Contains(methodTrait))
                {
                    matchedCount++;
                }
                else
                {
                    extraCount++;
                }
            }

            if (matchedCount == 0) return ResolutionOutcome.Failure;
            
            int missingCount = demonResolutionTraits.Count - matchedCount;
            bool matchedAllRequired = missingCount == 0;
            
            if (matchedAllRequired && extraCount == 0) return ResolutionOutcome.Success;

            if (matchedAllRequired && extraCount > 0) return ResolutionOutcome.PartialSuccess;

            return ResolutionOutcome.PartialFailure;
        }
    }
}
