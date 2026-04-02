using System.Linq;
using _Project.Scripts.Core.Data.Demons;
using _Project.Scripts.Core.Data.Investigation;

namespace _Project.Scripts.Systems
{
    public static class DeductionEvaluator
    {
        public static bool IsDemonExcluded(DemonDefinition demon, InvestigationNotebook notebook)
        {
            if (demon == null) return false;
            if (notebook == null) return false;

            for (int i = 0; i < notebook.Entries.Count; i++)
            {
                ClueNotebookEntry entry = notebook.Entries[i];
                if (entry == null) continue;
                if (entry.AssignedTrait == null) continue;

                bool demonHasTrait = demon.Traits.Contains(entry.AssignedTrait);

                switch (entry.EvaluationState)
                {
                    case ClueEvaluationState.Legit:
                        if (!demonHasTrait) return true;
                        break;
                    
                    case ClueEvaluationState.FalseLead:
                        if (demonHasTrait) return true;
                        break;
                    
                    case ClueEvaluationState.Unsure:
                        break;
                }
            }
            
            return false;
        }
    }
}
