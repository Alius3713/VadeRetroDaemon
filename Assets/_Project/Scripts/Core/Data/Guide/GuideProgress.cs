namespace _Project.Scripts.Core.Data.Guide
{
    public class GuideProgress
    {
        private readonly CaseGuideDefinition _guideDefinition;
        private int _currentObjectiveIndex;

        public GuideProgress(CaseGuideDefinition guideDefinition)
        {
            _guideDefinition = guideDefinition;
            _currentObjectiveIndex = 0;
        }
        
        public CaseGuideDefinition GuideDefinition => _guideDefinition;
        public int CurrentObjectiveIndex => _currentObjectiveIndex;

        public GuideObjectiveDefinition CurrentObjective
        {
            get
            {
                if (_guideDefinition == null) return null;
                if (_currentObjectiveIndex < 0 || _currentObjectiveIndex >= _guideDefinition.Objectives.Count) return null;
                
                return _guideDefinition.Objectives[_currentObjectiveIndex];
            }
        }

        public bool IsCompleted
        {
            get
            {
                if (_guideDefinition == null) return true;
                return _currentObjectiveIndex >= _guideDefinition.Objectives.Count;
            }
        }

        public bool TryAdvance(string completedObjectiveID)
        {
            GuideObjectiveDefinition currentObjective = CurrentObjective;
            if (currentObjective == null) return false;
            if (currentObjective.ObjectiveID != completedObjectiveID) return false;
            
            _currentObjectiveIndex++;
            return true;
        }
    }
}
