using DurkaSimRemastered;
using UnityEngine;


namespace Quests
{
    public class QuestObjectView : LevelObjectView
    {
        [SerializeField] private Color _completedColor;
        [SerializeField] private int _id;

        private Color _defaultColor;

        public int ID => _id;

        #region Methods

        private void Awake()
        {
            //_defaultColor = SpriteRenderer.color;
        }

        public virtual void ProcessComplete()
        {
            SpriteRenderer.color = _completedColor;
        }

        public void ProcessActivate()
        {
            //SpriteRenderer.color = _defaultColor;
        }

        #endregion
    }
}