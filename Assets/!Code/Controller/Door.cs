using System;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class Door : IExecute, ICleanup
    {
        public Action<DoorView> OnDoorActivated = delegate(DoorView pairDoorView) {  };
        
        private Collider2D _otherCollider;
        private PlayerView _playerView;

        private readonly DoorView _doorView;
        private readonly DoorView _pairDoorView;
        private readonly PlayerDataModel _playerDataModel;
        private readonly InputModel _inputModel;

        public bool IsPlayerNear { get; private set; }

        public Door(DoorView doorView, PlayerDataModel playerDataModel, 
            InputModel inputModel)
        {
            _doorView = doorView;
            _pairDoorView = _doorView.PairDoor;
            _playerDataModel = playerDataModel;
            _inputModel = inputModel;
            _doorView.OnTriggerEnter += OnTriggerEnter;
            _doorView.OnTriggerExit += OnTriggerExit;
        }

        #region Methods

        private void OnTriggerEnter(Collider2D other)
        {
            _playerDataModel.PlayerIntersects = true;
            IsPlayerNear = true;
            _otherCollider = other;
        }

        private void OnTriggerExit(Collider2D other)
        {
            _playerDataModel.PlayerIntersects = false;
            IsPlayerNear = false;
            _otherCollider = null;
        }
        
        public void Execute(float deltaTime)
        {
            if (IsPlayerNear)
            {
                if (_inputModel.GetInteractButtonDown)
                {
                    var readyToActivate = TryComplete(_otherCollider.gameObject);
                    if (readyToActivate)
                    {
                        Activate();
                    }
                }
            }
        }

        private bool TryComplete(GameObject other)
        {
            return other.TryGetComponent(out _playerView);
        }

        private void Activate()
        {
            OnDoorActivated.Invoke(_pairDoorView);
            _pairDoorView.OnTriggerEnter2D(_otherCollider);
            _doorView.AudioSource.Play();
        }

        public void Cleanup()
        {
            _doorView.OnTriggerEnter -= OnTriggerEnter;
            _doorView.OnTriggerExit -= OnTriggerExit;
        }

        #endregion
    }
}