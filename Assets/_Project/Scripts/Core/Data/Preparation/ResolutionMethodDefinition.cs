using System.Collections.Generic;
using _Project.Scripts.Core.Data.Demons;
using _Project.Scripts.Core.Data.Tools;
using UnityEngine;

namespace _Project.Scripts.Core.Data.Preparation
{
    [CreateAssetMenu(fileName = "NewResolutionMethod", menuName = "Game/Resolution/Resolution Method")]
    public class ResolutionMethodDefinition : ScriptableObject
    {
        [SerializeField] private string methodID;
        [SerializeField] private string displayName;
        [SerializeField][TextArea] private string description;
        [SerializeField] private List<ToolDefinition> requiredTools = new();
        [SerializeField] private List<TraitDefinition> supportedResolutionTraits = new();
        
        public string MethodID => methodID;
        public string DisplayName => displayName;
        public string Description => description;
        public IReadOnlyList<ToolDefinition> RequiredTools => requiredTools;
        public IReadOnlyList<TraitDefinition> SupportedResolutionTraits => supportedResolutionTraits;
    }
}
