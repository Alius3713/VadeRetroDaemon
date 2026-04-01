using UnityEngine;

namespace _Project.Scripts.Core.Data.Investigation
{
    public class InvestigationTarget : MonoBehaviour
    {
        [SerializeField] private ClueFragmentDefinition clueFragment;
        
        public ClueFragmentDefinition ClueFragment => clueFragment;
    }
}
