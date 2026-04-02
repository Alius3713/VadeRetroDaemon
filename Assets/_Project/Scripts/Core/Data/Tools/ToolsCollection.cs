using System.Collections.Generic;

namespace _Project.Scripts.Core.Data.Tools
{
    public class ToolsCollection
    {
        private readonly List<ToolEntry> _entries = new();
        
        public IReadOnlyList<ToolEntry> Entries => _entries;

        public void AddEntry(ToolEntry entry)
        {
            if (entry == null || entry.Tool == null || Contains(entry.Tool)) return;
            
            _entries.Add(entry);
        }

        public bool Contains(ToolDefinition tool)
        {
            if (tool == null) return false;

            for (int i = 0; i < _entries.Count; i++)
            {
                ToolEntry entry = _entries[i];
                if (entry == null) continue;
                if (entry.Tool == tool) return true;
            }
            
            return false;
        }

        public ToolEntry GetEntry(ToolDefinition tool)
        {
            if (tool == null) return null;

            for (int i = 0; i < _entries.Count; i++)
            {
                ToolEntry entry = _entries[i];
                if (entry == null) continue;
                if (entry.Tool == tool) return entry;
            }
            
            return null;
        }

        public bool IsUnlocked(ToolDefinition tool)
        {
            ToolEntry entry = GetEntry(tool);
            return entry != null && entry.IsUnlocked;
        }
        
        public void Unlock(ToolDefinition tool)
        {
            ToolEntry entry = GetEntry(tool);
            if (entry == null) return;
            
            entry.IsUnlocked = true;
        }
    }
}
