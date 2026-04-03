using UnityEngine;

namespace _Project.Scripts.Core.Data.Tools
{
    [CreateAssetMenu(fileName = "NewTool", menuName = "Game/Tools/Tool")]
    public class ToolDefinition : ScriptableObject
    {
        [SerializeField] private string toolID;
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField][TextArea] private string description;
        [SerializeField] private bool isUnlockedByDefault = false;
        
        public string ToolID => toolID;
        public Sprite Icon => icon;
        public string DisplayName => displayName;
        public string Description => description;
        public bool IsUnlockedByDefault => isUnlockedByDefault;
    }
}
