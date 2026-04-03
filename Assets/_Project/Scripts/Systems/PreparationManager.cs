using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Data.Preparation;
using _Project.Scripts.Core.Data.Tools;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    public class PreparationManager : MonoBehaviour
    {
        public static PreparationManager Instance { get; private set; }

        [Header("Data")]
        [SerializeField] private List<ResolutionMethodDefinition> allMethods = new();
        
        private PreparationLoadout _currentLoadout;
        
        public PreparationLoadout CurrentLoadout  => _currentLoadout;

        public event Action OnPreparationInitialized;
        public event Action OnPreparationChanged;
        public event Action<PreparationLoadout> OnPreparationConfirmed;
        public event Action OnPreparationCleared;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializePreparation();
        }

        public void InitializePreparation()
        {
            _currentLoadout = new PreparationLoadout();
            OnPreparationInitialized?.Invoke();
            OnPreparationChanged?.Invoke();
        }

        // public bool TryAddTool(ToolDefinition tool)
        // {
        //     if (_currentLoadout == null) return false;
        //     
        //     bool added = _currentLoadout.TryAddTool(tool);
        //     if (!added) return false;
        //     
        //     OnPreparationChanged?.Invoke();
        //     return true;
        // }
        //
        // public bool RemoveTool(ToolDefinition tool)
        // {
        //     if (_currentLoadout == null) return false;
        //     
        //     bool removed = _currentLoadout.RemoveTool(tool);
        //     if (!removed) return false;
        //     
        //     OnPreparationChanged?.Invoke();
        //     return true;
        // }

        public bool TryAssignToolToSlot(ToolDefinition tool, int slotIndex)
        {
            if (_currentLoadout == null) return false;
            bool assigned = _currentLoadout.TryAssignToolToSlot(tool, slotIndex);
            if (!assigned) return false;
            
            OnPreparationChanged?.Invoke();
            return true;
        }

        public bool TryAddToolToFirstEmptySlot(ToolDefinition tool)
        {
            if (_currentLoadout == null) return false;
            
            bool added = _currentLoadout.TryAddTooFirstEmptySlot(tool);
            if (!added) return false;
            
            OnPreparationChanged?.Invoke();
            return true;
        }

        public bool RemoveToolFromSlot(int slotIndex)
        {
            if (_currentLoadout == null) return false;
            
            bool removed = _currentLoadout.RemoveToolFromSlot(slotIndex);
            if (!removed) return false;
            
            OnPreparationChanged?.Invoke();
            return true;
        }
        
        public void SetSelectedMethod(ResolutionMethodDefinition method)
        {
            if (_currentLoadout == null) return;
            
            _currentLoadout.SetSelectedMethod(method);
            OnPreparationChanged?.Invoke();
        }

        public void SetMaxToolSlots(int maxToolSlots)
        {
            if (_currentLoadout == null) return;
            
            _currentLoadout.SetMaxToolSlots(maxToolSlots);
            OnPreparationChanged?.Invoke();
        }

        public IReadOnlyList<ResolutionMethodDefinition> GetAvailableMethods()
        {
            if (_currentLoadout == null) return Array.Empty<ResolutionMethodDefinition>();
            
            List<ResolutionMethodDefinition> availableMethods = new();

            for (int i = 0; i < allMethods.Count; i++)
            {
                ResolutionMethodDefinition method = allMethods[i];
                if (method == null) continue;
                
                if (_currentLoadout.CanUseMethod(method)) availableMethods.Add(method);
            }
            
            return availableMethods;
        }

        public bool CanConfirmPreparation()
        {
            if (_currentLoadout == null) return false;
            return _currentLoadout.IsReadyForConfirmation();
        }

        public bool ConfirmPreparation()
        {
            if (!CanConfirmPreparation()) return false;
            
            OnPreparationConfirmed?.Invoke(_currentLoadout);
            return true;
        }

        public void ClearAndReinitialize()
        {
            _currentLoadout = null;
            OnPreparationCleared?.Invoke();
            
            InitializePreparation();
        }
    }
}
