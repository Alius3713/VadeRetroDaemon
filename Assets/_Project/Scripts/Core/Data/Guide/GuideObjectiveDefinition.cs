using UnityEngine;

namespace _Project.Scripts.Core.Data.Guide
{
    [CreateAssetMenu(fileName = "NewGuideObjective", menuName = "Game/Guide/Guide Objective")]
    public class GuideObjectiveDefinition : ScriptableObject
    {
        [SerializeField] private string objectiveID;
        [SerializeField] private string title;
        [SerializeField] [TextArea] private string description;

        public string ObjectiveID => objectiveID;
        public string Title => title;
        public string Description => description;
    }
}
