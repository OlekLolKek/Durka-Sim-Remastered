using System;
using System.Collections.Generic;
using System.Linq;
using DurkaSimRemastered;
using UnityEngine;


namespace Quests
{
    public class QuestsConfigurator : MonoBehaviour
    {
        [SerializeField] private QuestObjectView _singleQuestView;
        [SerializeField] private QuestStoryConfig[] _questStoryConfigs;
        [SerializeField] private QuestObjectView[] _questObjects;

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
        private Quest _singleQuest;

        private void Start()
        {
            _singleQuest = new Quest(_singleQuestView, new SwitchQuestModel());
            _singleQuest.Reset();

            
            //TODO: figure out what this is
            _questStories = new List<IQuestStory>();
            foreach (var questStoryConfig in _questStoryConfigs)
            {
                _questStories.Add(CreateQuestStory(questStoryConfig));
            }
        }

        private void OnDestroy()
        {
            _singleQuest.Dispose();
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
                Debug.LogWarning($"QuestsConfigurator :: Start : Can't find view of quest {questId.ToString()}");
                return null;
            }

            if (_questFactories.TryGetValue(config.QuestType, out var factory))
            {
                var questModel = factory.Invoke();
                return new Quest(questView, questModel);
            }

            Debug.LogWarning($"QuestsConfigurator :: Start : Can't create model for quest {questId.ToString()}");
            return null;
        }
    }
}