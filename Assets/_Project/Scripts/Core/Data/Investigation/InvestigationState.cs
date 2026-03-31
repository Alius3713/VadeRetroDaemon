using System.Collections.Generic;

namespace _Project.Scripts.Core.Data.Investigation
{
    public class InvestigationState
    {
        private readonly List<ClueFragmentDefinition> _discoveredClues = new();
        
        public IReadOnlyList<ClueFragmentDefinition> DiscoveredClues => _discoveredClues;

        public void AddClue(ClueFragmentDefinition clue)
        {
            if (clue == null) return;
            if (_discoveredClues.Contains(clue)) return;
            
            _discoveredClues.Add(clue);
        }
    }
}
