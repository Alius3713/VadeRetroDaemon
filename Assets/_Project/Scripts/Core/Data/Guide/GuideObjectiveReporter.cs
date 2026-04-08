using _Project.Scripts.Systems;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Guide
{
    public class GuideObjectiveReporter : MonoBehaviour
    {
        [SerializeField] private string objectiveID;
        [SerializeField] private bool reportOnlyOnce = true;
        
        private bool _hasReported;

        public void ReportObjective()
        {
            if (reportOnlyOnce && _hasReported) return;
            if (string.IsNullOrWhiteSpace(objectiveID)) return;
            if (GuideManager.Instance == null) return;
            
            GuideManager.Instance.ReportObjectiveCompleted(objectiveID);
            _hasReported = true;
        }
    }
}
