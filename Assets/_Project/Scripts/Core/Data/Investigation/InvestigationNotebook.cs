using System.Collections.Generic;

namespace _Project.Scripts.Core.Data.Investigation
{
    public class InvestigationNotebook
    {
        private readonly List<ClueNotebookEntry> _entries = new();
        
        public IReadOnlyList<ClueNotebookEntry> Entries => _entries;

        public void AddEntry(ClueNotebookEntry entry)
        {
            if (entry == null) return;
            if (entry.SourceClue == null) return;
            
            if (ContainsEntryForClue(entry.SourceClue)) return;
            
            _entries.Add(entry);
        }

        public bool ContainsEntryForClue(ClueFragmentDefinition clueFragment)
        {
            if (clueFragment == null)  return false;

            foreach (ClueNotebookEntry entry in _entries)
            {
                if (entry == null) continue;
                if (entry.SourceClue == clueFragment) return true;
            }
            
            return false;
        }

        public ClueNotebookEntry GetEntryForClue(ClueFragmentDefinition clueFragment)
        {
            if (clueFragment == null) return null;

            foreach (ClueNotebookEntry entry in _entries)
            {
                if (entry ==  null) continue;
                if (entry.SourceClue == clueFragment) return entry;
            }
            
            return null;
        }

        public void Clear()
        {
            _entries.Clear();
        }
    }
}
