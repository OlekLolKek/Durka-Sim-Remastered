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
        private bool _isPlayerInTrigger;
        
        public event EventHandler<IQuest> Completed;
        public bool IsCompleted { get; private set; }

        #endregion

        public Quest(QuestObjectView view, IQuestModel model)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _model = model != null ? model : throw new ArgumentNullException(nameof(model));
        }

        #region Methods

        private void OnTriggerEnter(Collider2D other)
        {
            _isPlayerInTrigger = true;
            
            var completed = _model.TryComplete(other.gameObject);
            if (completed)
            {
                Complete();
            }
        }

        private void OnTriggerExit(Collider2D other)
        {
            _isPlayerInTrigger = false;
        }

        private void Complete()
        {
            if (!_active) return;
            _active = false;
            IsCompleted = true;
            _view.OnTriggerEnter -= OnTriggerEnter;
            _view.OnTriggerExit -= OnTriggerExit;
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
            _view.OnTriggerEnter += OnTriggerEnter;
            _view.OnTriggerExit += OnTriggerExit;
            _view.ProcessActivate();
        }
        
        public void Dispose()
        {
            _view.OnTriggerEnter -= OnTriggerEnter;
            _view.OnTriggerExit -= OnTriggerExit;
        }

        #endregion
    }
}