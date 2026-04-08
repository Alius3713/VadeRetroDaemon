using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Data.NPC
{
    [CreateAssetMenu(fileName = "NewNPC", menuName = "Game/NPC/Npc Data")]
    public class NpcDefinition : ScriptableObject
    {
        [SerializeField] private string npcID;
        [SerializeField] private string displayName;
        [SerializeField] private List<string> dialoguesIds;
        
        public string NpcID => npcID;
        public string DisplayName => displayName;
        public IReadOnlyList<string> DialoguesIds => dialoguesIds;
    }
}
