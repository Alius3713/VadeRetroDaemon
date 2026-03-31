using UnityEngine;

namespace _Project.Scripts.Core.Data.Demons
{
    [CreateAssetMenu(fileName = "NewTrait", menuName = "Game/Demons/Trait")]
    public class TraitDefinition : ScriptableObject
    {
        [SerializeField] private string traitID;
        [SerializeField] private string displayName;
        [SerializeField][TextArea] private string description;
        [SerializeField] private TraitCategory category;
        
        public string TraitID => traitID;
        public string DisplayName => displayName;
        public string Description => description;
        public TraitCategory Category => category;
    }
}
