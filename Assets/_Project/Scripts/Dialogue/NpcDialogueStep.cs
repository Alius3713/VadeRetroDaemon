using UnityEngine;

namespace _Project.Scripts.Dialogue
{
    [System.Serializable]
    public class NpcDialogueStep
    {
        [SerializeField] private string dialogId;
        [SerializeField] private bool playOnlyOnce = true;
        [SerializeField] private bool loopUntilConditionMet;
        [SerializeField] private string requiredObjectiveIdToAdvance;
        
        public string DialogId => dialogId;
        public bool PlayOnlyOnce => playOnlyOnce;
        public bool LoopUntilConditionMet => loopUntilConditionMet;
        public string RequiredObjectiveIdToAdvance => requiredObjectiveIdToAdvance;
    }
}
