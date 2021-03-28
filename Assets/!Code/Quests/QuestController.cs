using System;
using System.Collections.Generic;
using System.Linq;
using DurkaSimRemastered;
using DurkaSimRemastered.Interface;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Quests
{
    public class QuestController : IInitialize, ICleanup
    {
        private readonly QuestObjectView[] _singleQuestViews;
        private readonly QuestStoryConfig[] _questStoryConfigs;
        private readonly QuestObjectView[] _questObjects;

        private readonly Dictionary<QuestType, Func<IQuestModel>> _questFactories =
            new Dictionary<QuestType, Func<IQuestModel>>
            {
                {QuestType.Switch, () => new SwitchQuestModel()},
            };

        private readonly Dictionary<QuestStoryType, Func<List<IQuest>, IQuestStory>> _questStoryFactories =
            new Dictionary<QuestStoryType, Func<List<IQuest>, IQuestStory>>
            {
                {QuestStoryType.Common, questCollection => new QuestStory(questCollection)},
            };

        private List<IQuestStory> _questStories;
        private Quest[] _singleQuests;

        public QuestController()
        {
            var questSceneConfig = Object.FindObjectOfType<QuestsSceneConfig>();
            _singleQuestViews = questSceneConfig.SingleQuestViews;
            _questStoryConfigs = questSceneConfig.QuestStoryConfigs;
            _questObjects = questSceneConfig.QuestObjects;
        }
        
        public void Initialize()
        {
            _singleQuests = new Quest[_singleQuestViews.Length];
            for (int i = 0; i < _singleQuests.Length; i++)
            {
                _singleQuests[i] = new Quest(_singleQuestViews[i], new SwitchQuestModel());
                _singleQuests[i].Reset();
            }

            _questStories = new List<IQuestStory>();
            foreach (var questStoryConfig in _questStoryConfigs)
            {
                _questStories.Add(CreateQuestStory(questStoryConfig));
            }
        }

        public void Cleanup()
        {
            foreach (var questStory in _questStories)
            {
                questStory.Dispose();
            }

            _questStories.Clear();
            
            foreach (var singleQuest in _singleQuests)
            {
                singleQuest.Dispose();
            }
        }

        private IQuestStory CreateQuestStory(QuestStoryConfig config)
        {
            var quests = new List<IQuest>();
            foreach (var questConfig in config.Quests)
            {
                var quest = CreateQuest(questConfig);
                if (quest != null)
                {
                    quests.Add(quest);
                }
            }

            return _questStoryFactories[config.QuestStoryType].Invoke(quests);
        }

        private IQuest CreateQuest(QuestConfig config)
        {
            var questId = config.ID;
            var questView = _questObjects.FirstOrDefault(value => value.ID == config.ID);
            if (questView == null)
            {
                Debug.LogWarning($"{this} :: {nameof(Initialize)} : Can't find view of quest {questId.ToString()}");
                return null;
            }

            if (_questFactories.TryGetValue(config.QuestType, out var factory))
            {
                var questModel = factory.Invoke();
                return new Quest(questView, questModel);
            }

            Debug.LogWarning($"{this} :: {nameof(Initialize)} : Can't create model for quest {questId.ToString()}");
            return null;
        }
    }
}