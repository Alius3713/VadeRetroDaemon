using _Project.Scripts.Core.Data.Demons;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Cases
{
    [CreateAssetMenu(fileName = "NewCase", menuName = "Game/Cases/Case")]
    public class CaseDefinition : ScriptableObject
    {
        [SerializeField] private string caseID;
        [SerializeField] private string displayName;
        [SerializeField][TextArea] private string description;
        [SerializeField] private DemonDefinition demon;
        
        public string CaseID => caseID;
        public string DisplayName => displayName;
        public string Description => description;
        public DemonDefinition Demon => demon;
    }
}
