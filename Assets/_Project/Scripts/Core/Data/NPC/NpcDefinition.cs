using System.Collections.Generic;
using _Project.Scripts.Dialogue;
using UnityEngine;

namespace _Project.Scripts.Core.Data.NPC
{
    [CreateAssetMenu(fileName = "NewNPC", menuName = "Game/NPC/Npc Data")]
    public class NpcDefinition : ScriptableObject
    {
        [SerializeField] private string npcID;
        [SerializeField] private string displayName;
        [SerializeField] private List<NpcDialogueStep> dialogueSteps = new();
        
        public string NpcID => npcID;
        public string DisplayName => displayName;
        public IReadOnlyList<NpcDialogueStep> DialogueSteps => dialogueSteps;
    }
}
