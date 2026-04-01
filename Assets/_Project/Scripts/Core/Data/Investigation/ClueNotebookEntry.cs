using _Project.Scripts.Core.Data.Demons;

namespace _Project.Scripts.Core.Data.Investigation
{
    [System.Serializable]
    public class ClueNotebookEntry
    {
        public ClueFragmentDefinition SourceClue;
        public TraitDefinition AssignedTait;
        public ClueEvaluationState EvaluationState;
    }
}
