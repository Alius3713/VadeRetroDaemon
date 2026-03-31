using _Project.Scripts.Core.Data.Demons;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Investigation
{
    [System.Serializable]
    public class ClueTraitLink
    {
        [SerializeField] private TraitDefinition trait;
        [SerializeField] private ClueImpactType impactType;
        
        public TraitDefinition Trait => trait;
        public ClueImpactType ImpactType => impactType;
    }
}
