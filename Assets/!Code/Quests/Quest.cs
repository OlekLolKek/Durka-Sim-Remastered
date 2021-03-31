using System;
using DurkaSimRemastered;
using Model;
using UnityEngine;


namespace Quests
{
    public class Quest : IQuest
    {
        #region Fields

        private readonly PlayerInteractionModel _playerInteractionModel;
        private readonly QuestObjectView _view;
        private readonly IQuestModel _model;

        private bool _active;

        public event EventHandler<IQuest> Completed;
        public bool IsCompleted { get; private set; }

        #endregion

        public Quest(QuestObjectView view, IQuestModel model, PlayerInteractionModel playerInteractionModel)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _model = model != null ? model : throw new ArgumentNullException(nameof(model));
            _playerInteractionModel = playerInteractionModel != null
                ? playerInteractionModel
                : throw new ArgumentNullException(nameof(playerInteractionModel));
        }

        #region Methods

        private void OnTriggerEnter(Collider2D other)
        {
            _playerInteractionModel.PlayerIntersects = true;
            
            var completed = _model.TryComplete(other.gameObject);
            if (completed)
            {
                Complete();
            }
        }

        private void OnTriggerExit(Collider2D other)
        {
            _playerInteractionModel.PlayerIntersects = false;
            if (IsCompleted)
            {
                _view.OnTriggerExit -= OnTriggerExit;
            }
        }

        private void Complete()
        {
            if (!_active) return;
            _active = false;
            IsCompleted = true;
            _view.OnTriggerEnter -= OnTriggerEnter;
            _view.ProcessComplete();
            OnCompleted();
        }

        private void OnCompleted()
        {
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