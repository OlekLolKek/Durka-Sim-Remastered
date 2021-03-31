using DurkaSimRemastered;
using UnityEngine;


namespace Quests
{
    public sealed class SwitchQuestModel : IQuestModel
    {
        public bool TryComplete(GameObject activator)
        {
            return activator.TryGetComponent(out PlayerView _);
        }
    }
}