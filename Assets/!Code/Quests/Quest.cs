using System;
using DurkaSimRemastered;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace Quests
{
    public class Quest : IQuest
    {
        #region Fields

        private readonly PlayerDataModel _playerDataModel;
        private readonly QuestObjectView _view;
        private readonly InputModel _inputModel;
        private readonly IQuestModel _model;

        private Collider2D _otherCollider;
        private bool _active;

        public event EventHandler<IQuest> Completed;
        public bool IsCompleted { get; private set; }
        public bool IsPlayerNear { get; private set; }

        #endregion

        public Quest(QuestObjectView view, IQuestModel model,
            PlayerDataModel playerInteractionModel,
            InputModel inputModel)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _model = model != null ? model : throw new ArgumentNullException(nameof(model));
            _playerDataModel = playerInteractionModel != null
                ? playerInteractionModel
                : throw new ArgumentNullException(nameof(playerInteractionModel));
            _inputModel = inputModel;
        }

        #region Methods

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerView _))
            {
                _playerDataModel.PlayerIntersects = true;
                IsPlayerNear = true;
                _otherCollider = other;
            }
        }

        private void OnTriggerExit(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerView _))
            {
                _playerDataModel.PlayerIntersects = false;
                IsPlayerNear = false;
                _otherCollider = null;
            }
        }

        public void Execute(float deltaTime)
        {
            if (_inputModel.GetInteractButtonDown)
            {
                var completed = _model.TryComplete(_otherCollider.gameObject);
                if (completed)
                {
                    Complete();
                }
            }
        }

        private void Complete()
        {
            if (!_active) return;
            _playerDataModel.PlayerIntersects = false;
            _active = false;
            IsCompleted = true;
            _view.OnTriggerEnter -= OnTriggerEnter;
            _view.OnTriggerExit -= OnTriggerExit;
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