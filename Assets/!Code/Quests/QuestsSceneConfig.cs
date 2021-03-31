using System;
using System.Collections.Generic;
using DurkaSimRemastered;
using UnityEngine;


namespace Quests
{
    public class QuestsSceneConfig : MonoBehaviour
    {
        [SerializeField] private QuestObjectView[] _singleQuestViews;
        [SerializeField] private QuestStoryConfig[] _questStoryConfigs;
        [SerializeField] private QuestObjectView[] _questObjects;

        public QuestObjectView[] SingleQuestViews => _singleQuestViews;
        public QuestStoryConfig[] QuestStoryConfigs => _questStoryConfigs;
        public QuestObjectView[] QuestObjects => _questObjects;
    }
}