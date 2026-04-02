using System.Collections.Generic;

namespace _Project.Scripts.Core.Data.Demons
{
    public class Demonarium
    {
        private readonly List<DemonariumEntry> _entries = new();
        
        public IReadOnlyList<DemonariumEntry> Entries => _entries;

        public void AddEntry(DemonariumEntry entry)
        {
            if (entry == null || entry.Demon == null || Contains(entry.Demon)) return;
            
            _entries.Add(entry);
        }

        public bool Contains(DemonDefinition demon)
        {
            if (demon == null) return false;

            foreach (DemonariumEntry entry in _entries)
            {
                if (entry == null) continue;
                if (entry.Demon == demon) return true;
            }
            
            return false;
        }

        public DemonariumEntry GetEntry(DemonDefinition demon)
        {
            if (demon == null) return null;

            foreach (DemonariumEntry entry in _entries)
            {
                if (entry == null) continue;
                if (entry.Demon == demon) return entry;
            }
            
            return null;
        }

        public bool IsUnlocked(DemonDefinition demon)
        {
            DemonariumEntry entry = GetEntry(demon);
            return entry != null && entry.IsUnlocked;
        }

        public void Unlock(DemonDefinition demon)
        {
            DemonariumEntry entry = GetEntry(demon);
            if (entry == null)  return;
            
            entry.IsUnlocked = true;
        }
    }
}
