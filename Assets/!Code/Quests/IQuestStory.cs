using System;


namespace Quests
{
    public interface IQuestStory : IDisposable
    {
        bool IsDone { get; }
    }
}