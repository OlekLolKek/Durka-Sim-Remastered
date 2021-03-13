using System;
using DurkaSimRemastered.Interface;


namespace DurkaSimRemastered
{
    public class Elevator : IExecute
    {
        private readonly ElevatorView _view;
        private readonly float _maxHeight;
        private readonly float _minHeight;
            
        private const float IDLE_TIME = 0.5f;
        
        private ElevatorState _state = ElevatorState.IdleDown;
        private float _idleTimer;
        private float _speed = 3.0f;

        public Elevator(ElevatorView view)
        {
            _view = view;
            _maxHeight = _view.MAXHeight;
            _minHeight = _view.MINHeight;
        }
        
        public void Execute(float deltaTime)
        {
            Move(deltaTime);
        }

        private void Move(float deltaTime)
        {
            switch (_state)
            {
                case ElevatorState.IdleUp:
                    IdleUp(deltaTime);
                    break;
                case ElevatorState.IdleDown:
                    IdleDown(deltaTime);
                    break;
                case ElevatorState.GoingDown:
                    GoDown(deltaTime);
                    break;
                case ElevatorState.GoingUp:
                    GoUp(deltaTime);
                    break;
                case ElevatorState.None:
                    throw new Exception("Seems like the elevator is broken.");
            }
        }

        private void IdleUp(float deltaTime)
        {
            _idleTimer -= deltaTime;
            if (_idleTimer <= 0)
            {
                _state = ElevatorState.GoingDown;
                _idleTimer = IDLE_TIME;
            }
        }
        
        private void IdleDown(float deltaTime)
        {
            _idleTimer -= deltaTime;
            if (_idleTimer <= 0)
            {
                _state = ElevatorState.GoingUp;
                _idleTimer = IDLE_TIME;
            }
        }
        
        //TODO: change to DOTween if something goes wrong
        private void GoUp(float deltaTime)
        {
            if (_view.transform.position.y >= _maxHeight)
            {
                var fixedPosition = _view.transform.position;
                fixedPosition.y = _maxHeight;
                _view.transform.position = fixedPosition;
                _state = ElevatorState.IdleUp;
                return;
            }

            _view.transform.Translate(_view.transform.up * (_speed * deltaTime));
        }

        private void GoDown(float deltaTime)
        {
            if (_view.transform.position.y <= _minHeight)
            {
                var fixedPosition = _view.transform.position;
                fixedPosition.y = _minHeight;
                _view.transform.position = fixedPosition;
                _state = ElevatorState.IdleDown;
                return;
            }

            _view.transform.Translate(-_view.transform.up * (_speed * deltaTime));
        }
    }
}