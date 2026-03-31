using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Demons
{
    [CreateAssetMenu(fileName = "NewDemon", menuName = "Game/Demons/Demon")]
    public class DemonDefinition : ScriptableObject
    {
        [SerializeField] private string demonID;
        [SerializeField] private string displayName;
        [SerializeField][TextArea]  private string description;
        [SerializeField] private List<TraitDefinition> traits = new();

        public string DemonID => demonID;
        public string DisplayName => displayName;
        public string Description => description;
        public IReadOnlyList<TraitDefinition> Traits => traits;
    }
}
