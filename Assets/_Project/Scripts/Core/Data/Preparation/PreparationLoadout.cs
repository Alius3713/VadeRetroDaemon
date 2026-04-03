using System.Collections.Generic;
using _Project.Scripts.Core.Data.Tools;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Preparation
{
    public class PreparationLoadout
    {
        private readonly List<ToolDefinition> _toolSlots = new();
        
        public IReadOnlyList<ToolDefinition> ToolSlots => _toolSlots;
        public ResolutionMethodDefinition SelectedMethod { get; private set; }

        public int MaxToolSlots { get; private set; } = 2;

        public PreparationLoadout()
        {
            InitializeSlots(MaxToolSlots);
        }
        
        // public bool TryAddTool(ToolDefinition tool)
        // {
        //     if (tool == null) return false;
        //     if (_toolSlots.Contains(tool)) return false;
        //     if (_toolSlots.Count >= MaxToolSlots) return false;
        //     
        //     _toolSlots.Add(tool);
        //     return true;
        // }
        //
        // public bool RemoveTool(ToolDefinition tool)
        // {
        //     if (tool == null) return false;
        //     
        //     bool removed = _toolSlots.Remove(tool);
        //     if (removed && SelectedMethod != null && !CanUseMethod(SelectedMethod))
        //     {
        //         SelectedMethod = null;
        //     }
        //     
        //     return removed;
        // }

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
                if (!ContainsTool(requiredTool)) return false;
            }
            
            return true;
        }

        public void SetMaxToolSlots(int maxToolSlots)
        {
            int clampedSlots = Mathf.Clamp(maxToolSlots, 0, 5);
            
            if (clampedSlots == MaxToolSlots) return;
            
            List<ToolDefinition> previousTools = new(_toolSlots);
            
            MaxToolSlots = clampedSlots;
            _toolSlots.Clear();
            InitializeSlots(MaxToolSlots);
            
            int writeIndex = 0;
            for (int i = 0; i < previousTools.Count && writeIndex < _toolSlots.Count; i++)
            {
                ToolDefinition tool = previousTools[i];
                if (tool == null) continue;
                
                _toolSlots[writeIndex] = tool;
                writeIndex++;
            }
            
            if (SelectedMethod != null && !CanUseMethod(SelectedMethod))
            {
                SelectedMethod = null;
            }

            // MaxToolSlots = Mathf.Clamp(maxToolSlots, 2, 5);
            //
            // if (_toolSlots.Count > MaxToolSlots)
            // {
            //     _toolSlots.RemoveRange(MaxToolSlots, _toolSlots.Count - MaxToolSlots);
            // }
            //
        }
        public bool TryAssignToolToSlot(ToolDefinition tool, int slotIndex)
        {
            if (tool == null) return false;
            if (slotIndex < 0 || slotIndex >= _toolSlots.Count) return false;
            
            int existingSlotIndex = GetSlotIndexOfTool(tool);
            if (existingSlotIndex >= 0)
            {
                if (existingSlotIndex == slotIndex) return false;
                
                _toolSlots[existingSlotIndex] = null;
            }

            _toolSlots[slotIndex] = tool;
            
            if (SelectedMethod != null && !CanUseMethod(SelectedMethod)) SelectedMethod = null;
            
            return true;
        }

        public bool TryAddTooFirstEmptySlot(ToolDefinition tool)
        {
            if (tool == null) return false;
            if (ContainsTool(tool)) return false;

            for (int i = 0; i < _toolSlots.Count; i++)
            {
                if (_toolSlots[i] == null)
                {
                    _toolSlots[i] = tool;
                    
                    if (SelectedMethod != null && !CanUseMethod(SelectedMethod)) SelectedMethod = null;
                    
                    return true;
                }
            }
            
            return false;
        }

        public bool RemoveToolFromSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _toolSlots.Count) return false;
            if (_toolSlots[slotIndex] == null) return false;
            
            _toolSlots[slotIndex] = null;
            
            if (SelectedMethod != null && !CanUseMethod(SelectedMethod)) SelectedMethod = null;
            
            return true;
        }

        public ToolDefinition GetToolInSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _toolSlots.Count) return null;
            
            return _toolSlots[slotIndex];
        }
        
        public int GetSlotIndexOfTool(ToolDefinition tool)
        {
            if (tool == null) return -1;

            for (int i = 0; i < _toolSlots.Count; i++)
            {
                if (_toolSlots[i] == tool) return i;
            }
            
            return -1;
        }

        public bool ContainsTool(ToolDefinition tool)
        {
            return GetSlotIndexOfTool(tool) >= 0;
        }

        public int GetFilledSlotCount()
        {
            int count = 0;

            for (int i = 0; i < _toolSlots.Count; i++)
            {
                if (_toolSlots[i] != null) count++;
            }
            
            return count;
        }
        
        public bool IsReadyForConfirmation()
        {
            return SelectedMethod != null;
        }

        private void InitializeSlots(int slotCount)
        {
            for (int i = 0; i < slotCount; i++)
            {
                _toolSlots.Add(null);
            }
        }
    }
}
