using System.Collections.Generic;
using _Project.Scripts.Core.Data.Preparation;
using _Project.Scripts.Core.Data.Tools;
using _Project.Scripts.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Preparation
{
    public class PreparationPanelUI : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private Transform toolsCollectionContainer;
        [SerializeField] private ToolsCollectionItemUI toolsCollectionItemPrefab;
        
        [Header("Slots")]
        [SerializeField] private Transform toolSlotsContainer;
        [SerializeField] private PreparationToolSlotsUI toolSlotPrefab;
        
        [Header("Methods")]
        [SerializeField] private Transform methodsContainer;
        [SerializeField] private ResolutionMethodItemUI resolutionMethodItemPrefab;
        
        [Header("Info")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private ToolTooltipUI tooltipUI;

        private readonly List<ToolsCollectionItemUI> _spawnedToolItems = new();
        private readonly List<PreparationToolSlotsUI> _spawnedToolSlots = new();
        private readonly List<ResolutionMethodItemUI> _spawnedMethodItems = new();

        private ToolDefinition _selectedCollectionTool;

        private void Awake()
        {
            if (confirmButton != null)
            {
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(OnConfirmClicked);
            }

            if (tooltipUI != null) tooltipUI.Hide();
        }

        private void OnEnable()
        {
            SubscribeToEvents();
            Refresh();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
            
            if (tooltipUI != null) tooltipUI.Hide();
        }

        public void Refresh()
        {
            RefreshCollection();
            RefreshSlots();
            RefreshMethods();
            RefreshConfirmButtonState();
        }

        public void HandleCollectionToolClicked(ToolDefinition tool)
        {
            if (tool == null) return;
            if (PreparationManager.Instance == null) return;
            
            PreparationLoadout loadout = PreparationManager.Instance.CurrentLoadout;
            if (loadout == null) return;
            
            bool isCurrentlySelected = _selectedCollectionTool == tool;
            bool isAlreadyInLoadout = loadout.ContainsTool(tool);

            if (!isCurrentlySelected)
            {
                _selectedCollectionTool = tool;
                RefreshCollectionSelectionVisuals();
                return;
            }

            if (isAlreadyInLoadout)
            {
                _selectedCollectionTool = null;
                RefreshCollectionSelectionVisuals();
                return;
            }
            
            bool added = PreparationManager.Instance.TryAddToolToFirstEmptySlot(tool);
            
            if (!added) _selectedCollectionTool = null;
            
            RefreshCollectionSelectionVisuals();
        }

        public bool TryPlaceToolIntoSlot(ToolDefinition tool, int slotIndex)
        {
            if (PreparationManager.Instance == null) return false;
            if (tool == null) return false;
            
            return PreparationManager.Instance.TryAssignToolToSlot(tool, slotIndex);
        }
        
        public bool TryRemoveToolFromSlot(int slotIndex)
        {
            if (PreparationManager.Instance == null) return false;
            
            return PreparationManager.Instance.RemoveToolFromSlot(slotIndex);
        }
        
        public ToolDefinition GetToolInSlot(int slotIndex)
        {
            if (PreparationManager.Instance == null) return null;
            if (PreparationManager.Instance.CurrentLoadout == null) return null;

            return PreparationManager.Instance.CurrentLoadout.GetToolInSlot(slotIndex);
        }
        
        public void HandleMethodClicked(ResolutionMethodDefinition method)
        {
            if (PreparationManager.Instance == null) return;
            
            ResolutionMethodDefinition currentlySelectedMethod = PreparationManager.Instance.CurrentLoadout.SelectedMethod;
            if (currentlySelectedMethod == method)
            {
                PreparationManager.Instance.SetSelectedMethod(null);
                return;
            }
            
            if (method == null) return;
            PreparationManager.Instance.SetSelectedMethod(method);
        }

        public void ShowTooltip(ToolDefinition tool, Vector2 screenPosition)
        {
            if (tooltipUI == null || tool == null) return;
            
            tooltipUI.Show(tool, screenPosition);
        }

        public void HideTooltip()
        {
            if (tooltipUI == null) return;
            
            tooltipUI.Hide();
        }

        private void RefreshCollection()
        {
            ClearSpawnedToolItems();
            
            if (ToolsManager.Instance == null || PreparationManager.Instance == null) return;
            if (toolsCollectionContainer == null || toolsCollectionItemPrefab == null) return;
            
            IReadOnlyList<ToolDefinition> unlockedTools = ToolsManager.Instance.GetUnlockedTools();
            
            for (int i = 0; i < unlockedTools.Count; i++)
            {
                ToolDefinition tool = unlockedTools[i];
                if (tool == null) continue;
                
                ToolsCollectionItemUI item = Instantiate(toolsCollectionItemPrefab, toolsCollectionContainer);
                
                bool isSelectedInCollection = _selectedCollectionTool == tool;
                bool isInLoadout = PreparationManager.Instance.CurrentLoadout.ContainsTool(tool);
                
                item.Initialize(tool, this, isSelectedInCollection, isInLoadout);
                _spawnedToolItems.Add(item);
            }
        }

        private void RefreshSlots()
        {
            ClearSpawnedToolSlots();
            
            if (PreparationManager.Instance == null) return;
            if (toolSlotsContainer == null || toolSlotPrefab == null) return;

            PreparationLoadout loadout = PreparationManager.Instance.CurrentLoadout;
            if (loadout == null) return;

            for (int i = 0; i < loadout.MaxToolSlots; i++)
            {
                PreparationToolSlotsUI slot = Instantiate(toolSlotPrefab, toolSlotsContainer);
                slot.Initialize(this, i);
                slot.Bind(loadout.GetToolInSlot(i));
                _spawnedToolSlots.Add(slot);
            }
        }

        private void RefreshMethods()
        {
            ClearSpawnedMethodItems();
            
            if (PreparationManager.Instance == null) return;
            if (methodsContainer == null || resolutionMethodItemPrefab == null) return;
            
            IReadOnlyList<ResolutionMethodDefinition> availableMethods = PreparationManager.Instance.GetAvailableMethods();
            ResolutionMethodDefinition selectedMethod = PreparationManager.Instance.CurrentLoadout.SelectedMethod;

            for (int i = 0; i < availableMethods.Count; i++)
            {
                ResolutionMethodDefinition method = availableMethods[i];
                if (method == null) continue;
                
                ResolutionMethodItemUI item = Instantiate(resolutionMethodItemPrefab, methodsContainer);
                bool isSelected = selectedMethod == method;
                
                item.Initialize(method, this, isSelected);
                _spawnedMethodItems.Add(item);
            }
        }

        private void RefreshConfirmButtonState()
        {
            if (confirmButton == null || PreparationManager.Instance == null) return;

            confirmButton.interactable = PreparationManager.Instance.CanConfirmPreparation();
        }

        private void RefreshCollectionSelectionVisuals()
        {
            for (int i = 0; i < _spawnedToolItems.Count; i++)
            {
                if (_spawnedToolItems[i] == null) continue;

                ToolDefinition tool = _spawnedToolItems[i].Tool;
                bool isSelected = _selectedCollectionTool == tool;
                bool isInLoadout = PreparationManager.Instance != null && PreparationManager.Instance.CurrentLoadout.ContainsTool(tool);
                
                _spawnedToolItems[i].SetSelectedVisual(isSelected);
                _spawnedToolItems[i].SetInLoadoutVisual(isInLoadout);
            }
        }

        private void OnConfirmClicked()
        {
            if (PreparationManager.Instance == null) return;
            
            PreparationManager.Instance.ConfirmPreparation();
        }

        private void SubscribeToEvents()
        {
            if (ToolsManager.Instance != null)
            {
                ToolsManager.Instance.OnToolUnlocked += HandleToolUnlocked;
                ToolsManager.Instance.OnToolsInitialized += HandleToolsChanged;
                ToolsManager.Instance.OnToolsCleared += HandleToolsChanged;
            }
            if (PreparationManager.Instance != null)
            {
                PreparationManager.Instance.OnPreparationInitialized += HandlePreparationChanged;
                PreparationManager.Instance.OnPreparationChanged += HandlePreparationChanged;
                PreparationManager.Instance.OnPreparationCleared += HandlePreparationChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (ToolsManager.Instance != null)
            {
                ToolsManager.Instance.OnToolUnlocked -= HandleToolUnlocked;
                ToolsManager.Instance.OnToolsInitialized -= HandleToolsChanged;
                ToolsManager.Instance.OnToolsCleared -= HandleToolsChanged;
            }
            if (PreparationManager.Instance != null)
            {
                PreparationManager.Instance.OnPreparationInitialized -= HandlePreparationChanged;
                PreparationManager.Instance.OnPreparationChanged -= HandlePreparationChanged;
                PreparationManager.Instance.OnPreparationCleared -= HandlePreparationChanged;
            }
        }

        private void HandleToolUnlocked(ToolDefinition tool)
        {
            Refresh();
        }

        private void HandleToolsChanged()
        {
            Refresh();
        }

        private void HandlePreparationChanged()
        {
            Refresh();
        }

        private void ClearSpawnedToolItems()
        {
            for (int i = 0; i < _spawnedToolItems.Count; i++)
            {
                if (_spawnedToolItems[i] != null)
                {
                    Destroy(_spawnedToolItems[i].gameObject);
                }
            }
            
            _spawnedToolItems.Clear();
        }

        private void ClearSpawnedToolSlots()
        {
            for (int i = 0; i < _spawnedToolSlots.Count; i++)
            {
                if (_spawnedToolSlots[i] != null)
                {
                    Destroy(_spawnedToolSlots[i].gameObject);
                }
            }
            
            _spawnedToolSlots.Clear();
        }

        private void ClearSpawnedMethodItems()
        {
            for (int i = 0; i < _spawnedMethodItems.Count; i++)
            {
                if (_spawnedMethodItems[i] != null)
                {
                    Destroy(_spawnedMethodItems[i].gameObject);
                }
            }
            
            _spawnedMethodItems.Clear();
        }

        public void ClearSelectedToolIfMatches(ToolDefinition tool)
        {
            if (_selectedCollectionTool != tool) return;
            
            _selectedCollectionTool = null;
            RefreshCollectionSelectionVisuals();
        }
    }
}
