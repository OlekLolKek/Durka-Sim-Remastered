using UnityEngine;


namespace DurkaSimRemastered
{
    [CreateAssetMenu(fileName = "QuestStoryConfig", menuName = "Configs/QuestStoryConfig", order = 0)]
    public class QuestStoryConfig : ScriptableObject
    {
        [SerializeField] private QuestConfig[] _quests;
        [SerializeField] private QuestStoryType _questStoryType;

        public QuestConfig[] Quests => _quests;
        public QuestStoryType QuestStoryType => _questStoryType;
    }
}