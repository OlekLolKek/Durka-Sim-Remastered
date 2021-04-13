using System;
using System.Collections;
using DurkaSimRemastered.Interface;
using Model;
using UniRx;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class CreditsPlayerController : IExecute, ICleanup
    {
        private const float WALK_SPEED = 150.0f;
        private const float MOVING_THRESHOLD = 0.1f;
        private const float NORMAL_GRAVITY_SCALE = 1.0f;
        private const float EXECUTE_HEAD_TORQUE = 10.0f;
        private const float START_HEAD_TORQUE = 0.5f;
        
        private readonly SpriteAnimator _spriteAnimator;
        private readonly PlayerView _playerView;
        private readonly InputModel _inputModel;
        private readonly PlayerDataModel _playerDataModel;
        private readonly CreditsMovementModel _creditsMovementModel;
        private readonly LevelObjectView _johnLemonHead;

        private readonly Vector3 _leftScale = new Vector3(-1.0f, 1.0f, 1.0f);
        private readonly Vector3 _rightScale = new Vector3(1.0f, 1.0f, 1.0f);

        private float _horizontal;

        public CreditsPlayerController(PlayerView playerView,
            InputModel inputModel, SpriteAnimatorConfig playerAnimatorConfig,
            PlayerDataModel playerDataModel, CreditsMovementModel creditsMovementModel,
            LevelObjectView johnLemonHead)
        {
            _playerView = playerView;
            _inputModel = inputModel;
            _playerDataModel = playerDataModel;
            _creditsMovementModel = creditsMovementModel;
            _johnLemonHead = johnLemonHead;
            _spriteAnimator = new SpriteAnimator(playerAnimatorConfig);
            _spriteAnimator.StartAnimation(_playerView.SpriteRenderer, 
                AnimationState.Jump, true, 
                AnimationSpeeds.NORMAL_ANIMATION_SPEED);
            
            _creditsMovementModel.OnMovementFinished += EnableGravity;
            _johnLemonHead.Rigidbody2D.AddTorque(START_HEAD_TORQUE);
        }

        public void Execute(float deltaTime)
        {
            _horizontal = _inputModel.Horizontal;
            _spriteAnimator.Execute(deltaTime);
            Move();
            _johnLemonHead.transform.Rotate(Vector3.forward,
                EXECUTE_HEAD_TORQUE * deltaTime);
        }

        private void EnableGravity()
        {
            _playerView.Rigidbody2D.gravityScale = NORMAL_GRAVITY_SCALE;
            _playerView.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            _johnLemonHead.Rigidbody2D.gravityScale = NORMAL_GRAVITY_SCALE;
            _johnLemonHead.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        }

        private void Move()
        {
            var newVelocity = 0.0f;
            var isWalking = Mathf.Abs(_horizontal) > MOVING_THRESHOLD;

            if (isWalking)
            {
                if (_horizontal < 0)
                {
                    _playerView.transform.localScale = _leftScale;
                    _playerDataModel.IsPlayerFacingRight = false;
                }
                else
                {
                    _playerView.transform.localScale = _rightScale;
                    _playerDataModel.IsPlayerFacingRight = true;
                }

                newVelocity = Time.fixedDeltaTime * _horizontal * WALK_SPEED;
            }
            
            _playerView.Rigidbody2D.velocity =
                _playerView.Rigidbody2D.velocity.Change(x: newVelocity);
        }

        public void Cleanup()
        {
            _creditsMovementModel.OnMovementFinished -= EnableGravity;
        }
    }
}