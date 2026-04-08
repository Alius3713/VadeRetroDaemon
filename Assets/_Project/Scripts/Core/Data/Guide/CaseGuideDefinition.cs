using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Guide
{
    [CreateAssetMenu(fileName = "NewCaseGuide", menuName = "Game/Guide/Case Guide")]
    public class CaseGuideDefinition : ScriptableObject
    {
        [SerializeField] private string guideID;
        [SerializeField] private string displayName;
        [SerializeField] private List<GuideObjectiveDefinition> objectives = new();

        public string GuideId => guideID;
        public string DisplayName => displayName;
        public IReadOnlyList<GuideObjectiveDefinition> Objectives => objectives;
    }
}
