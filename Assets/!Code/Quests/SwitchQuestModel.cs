using UnityEngine;


namespace Quests
{
    public sealed class SwitchQuestModel : IQuestModel
    {
        private const string TARGET_TAG = "Player";
        
        public bool TryComplete(GameObject activator)
        {
            return activator.CompareTag(TARGET_TAG);
        }
    }
}