using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Data.Demons;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class DemonariumManager : MonoBehaviour
    {
        public static DemonariumManager Instance { get; private set; }

        [Header("Data")]
        [SerializeField] private List<DemonDefinition> allDemons = new();
        
        private Demonarium _currentDemonarium;
        
        public Demonarium CurrentDemonarium => _currentDemonarium;

        public event Action<DemonDefinition> OnDemonUnlocked;
        public event Action OnDemonariumInitialized;
        public event Action OnDemonariumCleared;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializeDemonarium();
        }

        public void InitializeDemonarium()
        {
            _currentDemonarium = new Demonarium();

            for (int i = 0; i < allDemons.Count; i++)
            {
                DemonDefinition demon = allDemons[i];
                if (demon == null) continue;

                DemonariumEntry entry = new DemonariumEntry
                {
                    Demon = demon,
                    IsUnlocked = demon.IsKnownByDefault
                };
                
                _currentDemonarium.AddEntry(entry);
            }
            
            OnDemonariumInitialized?.Invoke();
        }

        public IReadOnlyList<DemonariumEntry> GetAllEntries()
        {
            if (_currentDemonarium == null) return Array.Empty<DemonariumEntry>();

            return _currentDemonarium.Entries;
        }

        public IReadOnlyList<DemonDefinition> GetUnlockedDemons()
        {
            if (_currentDemonarium == null) return Array.Empty<DemonDefinition>();
            
            List<DemonDefinition> unlockedDemons = new();
            
            IReadOnlyList<DemonariumEntry> entries = _currentDemonarium.Entries;
            for (int i = 0; i < entries.Count; i++)
            {
                DemonariumEntry entry = entries[i];
                if (entry == null || !entry.IsUnlocked || entry.Demon == null) continue;
                
                unlockedDemons.Add(entry.Demon);
            }
            
            return unlockedDemons;
        }

        public bool IsUnlocked(DemonDefinition demon)
        {
            if (_currentDemonarium == null) return false;
            if (demon == null) return false;

            return _currentDemonarium.IsUnlocked(demon);
        }

        public void UnlockDemon(DemonDefinition demon)
        {
            if (_currentDemonarium == null || demon == null) return;

            DemonariumEntry entry = _currentDemonarium.GetEntry(demon);
            if (entry == null || entry.IsUnlocked) return;
            
            entry.IsUnlocked = true;
            OnDemonUnlocked?.Invoke(demon);
        }

        public DemonariumEntry GetEntry(DemonDefinition demon)
        {
            if (_currentDemonarium == null) return null;
            if (demon == null) return null;

            return _currentDemonarium.GetEntry(demon);
        }

        public void ClearAndReinitialize()
        {
            _currentDemonarium = null;
            OnDemonariumCleared?.Invoke();
            
            InitializeDemonarium();
        }
    }
}
