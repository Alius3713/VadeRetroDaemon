using System.Collections.Generic;
using _Project.Scripts.Core.Data.Tools;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Preparation
{
    public class PreparationLoadout
    {
        private readonly List<ToolDefinition> _selectedTools = new();
        
        public IReadOnlyList<ToolDefinition> SelectedTools => _selectedTools;
        public ResolutionMethodDefinition SelectedMethod { get; private set; }

        public int MaxToolSlots { get; private set; } = 2;

        public bool TryAddTool(ToolDefinition tool)
        {
            if (tool == null) return false;
            if (_selectedTools.Contains(tool)) return false;
            if (_selectedTools.Count >= MaxToolSlots) return false;
            
            _selectedTools.Add(tool);
            return true;
        }

        public bool RemoveTool(ToolDefinition tool)
        {
            if (tool == null) return false;
            
            bool removed = _selectedTools.Remove(tool);
            if (removed && SelectedMethod != null && !CanUseMethod(SelectedMethod))
            {
                SelectedMethod = null;
            }
            
            return removed;
        }

        public void SetSelectedMethod(ResolutionMethodDefinition method)
        {
            if (method == null)
            {
                SelectedMethod = null;
                return;
            }
            
            if (!CanUseMethod(method)) return;
            
            SelectedMethod = method;
        }

        public bool CanUseMethod(ResolutionMethodDefinition method)
        {
            if (method == null) return false;
            
            IReadOnlyList<ToolDefinition> requiredTools = method.RequiredTools;
            for (int i = 0; i < requiredTools.Count; i++)
            {
                ToolDefinition requiredTool = requiredTools[i];
                if (requiredTool == null) continue;
                if (!_selectedTools.Contains(requiredTool)) return false;
            }
            
            return true;
        }

        public void SetMaxToolSlots(int maxToolSlots)
        {
            MaxToolSlots = Mathf.Clamp(maxToolSlots, 2, 5);
            
            if (_selectedTools.Count > MaxToolSlots)
            {
                _selectedTools.RemoveRange(MaxToolSlots, _selectedTools.Count - MaxToolSlots);
            }

            if (SelectedMethod != null && !CanUseMethod(SelectedMethod))
            {
                SelectedMethod = null;
            }
        }

        public bool IsReadyForConfirmation()
        {
            return SelectedMethod != null;
        }
    }
}
