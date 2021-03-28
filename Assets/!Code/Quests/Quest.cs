using System;
using DurkaSimRemastered;
using UnityEngine;


namespace Quests
{
    public class Quest : IQuest
    {
        #region Fields

        private readonly QuestObjectView _view;
        private readonly IQuestModel _model;

        private bool _active;
        
        public event EventHandler<IQuest> Completed;
        public bool IsCompleted { get; private set; }

        #endregion

        public Quest(QuestObjectView view, IQuestModel model)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _model = model != null ? model : throw new ArgumentNullException(nameof(model));
        }

        #region Methods

        private void OnContact(Collider2D other)
        {
            //TODO: decide if this check is needed at all or using `other.gameObject` is fine
            
            if (!other.TryGetComponent(out LevelObjectView levelObjectView))
            {
                return;
            }
            var completed = _model.TryComplete(levelObjectView.gameObject);
            if (completed)
            {
                Complete();
            }
        }

        private void Complete()
        {
            if (!_active) return;
            _active = false;
            IsCompleted = true;
            _view.OnLevelObjectContact -= OnContact;
            _view.ProcessComplete();
            OnCompleted();
        }

        private void OnCompleted()
        {
            Debug.Log($"Quest {_view.ID} completed!");
            Completed?.Invoke(this, this);
        }

        public void Reset()
        {
            if (_active)
            {
                return;
            }

            _active = true;
            IsCompleted = false;
            _view.OnLevelObjectContact += OnContact;
            _view.ProcessActivate();
        }
        
        public void Dispose()
        {
            _view.OnLevelObjectContact -= OnContact;
        }

        #endregion
    }
}