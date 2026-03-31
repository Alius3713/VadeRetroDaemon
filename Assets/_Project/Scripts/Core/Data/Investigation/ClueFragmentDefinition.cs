using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Investigation
{
    [CreateAssetMenu(fileName = "NewClueFragment", menuName = "Game/Investigation/Clue Fragment")]
    public class ClueFragmentDefinition : ScriptableObject
    {
        [SerializeField] private string clueFragmentID;
        [SerializeField] private string displayName;
        [SerializeField][TextArea] private string description;
        [SerializeField] private List<ClueTraitLink> traitLinks = new();
        [SerializeField] private bool isMisleading;
        
        public string ClueFragmentID => clueFragmentID;
        public string DisplayName => displayName;
        public string Description => description;
        public IReadOnlyList<ClueTraitLink> TraitLinks => traitLinks;
        public bool IsMisleading => isMisleading;
    }
}
