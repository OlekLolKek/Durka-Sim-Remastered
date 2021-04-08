using System;
using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class Elevator : IExecute
    {
        private ElevatorState _state = ElevatorState.IdleDown;
        private float _idleTimer;
        
        private readonly ElevatorView _view;
        private readonly float _maxHeight;
        private readonly float _minHeight;
        
        private const float SPEED = 1.5f;
        private const float IDLE_TIME = 0.5f;

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
                    GoDown();
                    break;
                case ElevatorState.GoingUp:
                    GoUp();
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
        
        private void GoUp()
        {
            if (_view.transform.position.y >= _maxHeight)
            {
                _view.Rigidbody2D.velocity = Vector2.zero;
                _view.transform.position = _view.transform.position.Change(y: _maxHeight);
                _state = ElevatorState.IdleUp;
                return;
            }

            var newVelocity = new Vector2(0.0f, SPEED);
            _view.Rigidbody2D.velocity = newVelocity;
        }

        private void GoDown()
        {
            if (_view.transform.position.y <= _minHeight)
            {
                _view.Rigidbody2D.velocity = Vector2.zero;
                _view.transform.position = _view.transform.position.Change(y: _minHeight);
                _state = ElevatorState.IdleDown;
                return;
            }

            var newVelocity = new Vector2(0.0f, -SPEED);
            _view.Rigidbody2D.velocity = newVelocity;
        }
    }
}