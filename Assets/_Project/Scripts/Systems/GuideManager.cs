using System;
using _Project.Scripts.Core.Data.Guide;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class GuideManager : MonoBehaviour
    {
        public static GuideManager Instance { get; private set; }
        
        [SerializeField] private CaseGuideDefinition startingGuide;
        
        private GuideProgress _currentGuideProgress;
        
        public GuideProgress CurrentGuideProgress => _currentGuideProgress;
        
        public event Action<GuideObjectiveDefinition> OnObjectiveStarted;
        public event Action<GuideObjectiveDefinition> OnObjectiveCompleted;
        public event Action OnGuideCompleted;
        public event Action OnGuideCleared;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            if (startingGuide != null) StartGuide(startingGuide);
        }

        public void StartGuide(CaseGuideDefinition guideDefinition)
        {
            if (guideDefinition == null) return;
            
            _currentGuideProgress = new GuideProgress(guideDefinition);

            if (_currentGuideProgress.CurrentObjective != null)
            {
                OnObjectiveStarted?.Invoke(_currentGuideProgress.CurrentObjective);
            }
        }

        public void ReportObjectiveCompleted(string objectiveID)
        {
            if (_currentGuideProgress == null) return;
            if (string.IsNullOrWhiteSpace(objectiveID)) return;
            if (_currentGuideProgress.IsCompleted) return;
            
            GuideObjectiveDefinition completedObjective = _currentGuideProgress.CurrentObjective;
            
            bool advanced = _currentGuideProgress.TryAdvance(objectiveID);
            if (!advanced) return;
            
            OnObjectiveCompleted?.Invoke(completedObjective);

            if (_currentGuideProgress.IsCompleted)
            {
                OnGuideCompleted?.Invoke();
                return;
            }
            
            OnObjectiveStarted?.Invoke(_currentGuideProgress.CurrentObjective);
        }

        public bool IsCurrentObjective(string objectiveID)
        {
            if (_currentGuideProgress == null) return false;
            if (_currentGuideProgress.CurrentObjective == null) return false;
            
            return _currentGuideProgress.CurrentObjective.ObjectiveID == objectiveID;
        }

        public GuideObjectiveDefinition GetCurrentObjective()
        {
            return _currentGuideProgress?.CurrentObjective;
        }

        public void ClearGuide()
        {
            _currentGuideProgress = null;
            OnGuideCleared?.Invoke();
        }
    }
}
