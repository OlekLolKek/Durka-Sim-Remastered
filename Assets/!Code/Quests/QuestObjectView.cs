using DurkaSimRemastered;
using UnityEngine;


namespace Quests
{
    public class QuestObjectView : LevelObjectView
    {
        [SerializeField] protected int _id;

        public int ID => _id;

        #region Methods

        public virtual void ProcessComplete()
        {
        }

        public virtual void ProcessActivate()
        {
        }

        #endregion
    }
}