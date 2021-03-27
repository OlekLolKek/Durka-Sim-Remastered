using UnityEngine;


namespace Quests
{
    public interface IQuestModel
    {
        bool TryComplete(GameObject activator);
    }
}