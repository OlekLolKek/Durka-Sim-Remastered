using System;
using DurkaSimRemastered.Interface;


namespace Quests
{
    public interface IQuestStory : IExecute, IDisposable 
    {
        bool IsDone { get; }
    }
}