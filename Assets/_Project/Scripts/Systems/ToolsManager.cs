using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Data.Tools;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class ToolsManager : MonoBehaviour
    {
        public static ToolsManager Instance { get; private set; }

        [Header("Data")]
        [SerializeField] private List<ToolDefinition> allTools = new();
        
        private ToolsCollection _currentToolsCollection;
        
        public ToolsCollection CurrentToolsCollection => _currentToolsCollection;
        
        public event Action<ToolDefinition> OnToolUnlocked;
        public event Action OnToolsInitialized;
        public event Action OnToolsCleared;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializeTools();
        }

        public void InitializeTools()
        {
            _currentToolsCollection = new ToolsCollection();

            for (int i = 0; i < allTools.Count; i++)
            {
                ToolDefinition tool = allTools[i];
                if (tool == null) continue;

                ToolEntry entry = new ToolEntry
                {
                    Tool = tool,
                    IsUnlocked = tool.IsUnlockedByDefault
                };
                
                _currentToolsCollection.AddEntry(entry);
            }
            
            OnToolsInitialized?.Invoke();
        }

        public IReadOnlyList<ToolEntry> GetAllEntries()
        {
            if (_currentToolsCollection == null) return Array.Empty<ToolEntry>();
            return _currentToolsCollection.Entries;
        }

        public IReadOnlyList<ToolDefinition> GetUnlockedTools()
        {
            if (_currentToolsCollection == null) return Array.Empty<ToolDefinition>();
            
            List<ToolDefinition> unlockedTools = new();
            
            IReadOnlyList<ToolEntry> entries = _currentToolsCollection.Entries;
            for (int i = 0; i < entries.Count; i++)
            {
                ToolEntry entry = entries[i];
                if (entry == null) continue;
                if (!entry.IsUnlocked) continue;
                if (entry.Tool == null) continue;
                
                unlockedTools.Add(entry.Tool);
            }
            
            return unlockedTools;
        }

        public bool IsUnlocked(ToolDefinition tool)
        {
            if (_currentToolsCollection == null) return false;
            if (tool == null) return false;
            
            return _currentToolsCollection.IsUnlocked(tool);
        }

        public void UnlockTool(ToolDefinition tool)
        {
            if (_currentToolsCollection == null || tool == null) return;
            
            ToolEntry entry = _currentToolsCollection.GetEntry(tool);
            if (entry == null || entry.IsUnlocked) return;
            
            entry.IsUnlocked = true;
            OnToolUnlocked?.Invoke(tool);
        }

        public ToolEntry GetEntry(ToolDefinition tool)
        {
            if (_currentToolsCollection == null) return null;
            if (tool == null) return null;
            
            return _currentToolsCollection.GetEntry(tool);
        }

        public void ClearAndReinitialize()
        {
            _currentToolsCollection = null;
            OnToolsCleared?.Invoke();
            
            InitializeTools();
        }
    }
}
