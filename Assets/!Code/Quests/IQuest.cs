using System;
using DurkaSimRemastered.Interface;


namespace Quests
{
    public interface IQuest : IExecute, IDisposable
    {
        event EventHandler<IQuest> Completed;
        bool IsCompleted { get; }
        bool IsPlayerNear { get; }
        void Reset();
    }
}