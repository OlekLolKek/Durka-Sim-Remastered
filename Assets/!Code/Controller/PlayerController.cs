using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class PlayerController : IExecute, ICleanup
    {
        private readonly PlayerTeleportController _playerTeleportController;
        private readonly PlayerFallController _playerFallController;
        private readonly PlayerLifeController _playerLifeController;
        private readonly ShootController _shootController;

        private readonly PlayerDataModel _playerDataModel;
        private readonly SpriteAnimator _spriteAnimator;
        private readonly ContactPoller _contactPoller;
        private readonly InputModel _inputModel;
        private readonly PlayerLifeModel _playerLifeModel;
        private readonly PlayerView _playerView;

        private readonly Vector3 _leftScale = new Vector3(-1.0f, 1.0f, 1.0f);
        private readonly Vector3 _rightScale = new Vector3(1.0f, 1.0f, 1.0f);

        private const float WALK_SPEED = 150.0f;
        private const float JUMP_FORCE = 400.0f;
        private const float MOVING_THRESHOLD = 0.1f;

        private float _fallTimer;
        private bool _doJump;
        private float _horizontal;

        public PlayerController(PlayerView playerView, SpriteAnimatorConfig playerConfig,
            InputModel inputModel, PlayerLifeModel playerLifeModel,
            PlayerDataModel playerDataModel, List<BulletView> bullets,
            List<BulletEffectView> bulletParticles,
            Transform bulletSource, AmmoModel ammoModel,
            BulletConfig bulletConfig, Camera camera,
            DoorUseModel doorUseModel)
        {
            _playerDataModel = playerDataModel;
            _playerView = playerView;
            _inputModel = inputModel;
            _playerLifeModel = playerLifeModel;
            _spriteAnimator = new SpriteAnimator(playerConfig);
            _contactPoller = new ContactPoller(_playerView.Collider2D, _playerDataModel);

            _playerTeleportController = 
                new PlayerTeleportController(doorUseModel, _playerView, playerDataModel);
            
            _playerFallController = 
                new PlayerFallController(_contactPoller, _inputModel, _playerDataModel);
            
            _playerLifeController = 
                new PlayerLifeController(_playerView, playerLifeModel);
            
            _shootController = 
                new ShootController(bullets, bulletParticles, bulletSource,
                inputModel, ammoModel, bulletConfig, playerDataModel,
                camera, _playerView, _spriteAnimator);
        }

        public void Execute(float deltaTime)
        {
            _doJump = _inputModel.GetJumpButtonDown;
            _horizontal = _inputModel.Horizontal;

            _contactPoller.Execute(Time.fixedDeltaTime);

            //For some reason the Update method doesn't work correctly with Time.deltaTime, the velocity changes for some reason
            //Time.fixedDeltaTime works just fine, even though it shouldn't

            if (_playerLifeModel.IsDead)
            {
                Walk(false);
                return;
            }
            
            var isWalking = Mathf.Abs(_horizontal) > MOVING_THRESHOLD;
            
            Walk(isWalking);

            Jump();

            Animate(isWalking, deltaTime);

            _playerFallController.Execute(deltaTime);
            _shootController.Execute(deltaTime);
        }

        private void Walk(bool isWalking)
        {
            var newVelocity = 0.0f;

            if (_playerDataModel.IsPlayerShooting)
            {
                _playerView.Rigidbody2D.velocity = _playerView.Rigidbody2D.velocity.Change(x: newVelocity);
                return;
            }

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

                if ((_horizontal > 0 && !_playerDataModel.HasRightContacts)
                    || (_horizontal < 0 && !_playerDataModel.HasLeftContacts))
                {
                    newVelocity = Time.fixedDeltaTime * _horizontal * WALK_SPEED;
                }
            }

            _playerView.Rigidbody2D.velocity = _playerView.Rigidbody2D.velocity.Change(x: newVelocity);
        }

        private void Jump()
        {
            if (_playerDataModel.IsGrounded && _doJump)
            {
                _playerView.Rigidbody2D.AddForce(Vector3.up * JUMP_FORCE);
            }
        }

        private void Animate(bool isWalking, float deltaTime)
        {
            if (!_playerDataModel.IsPlayerShooting)
            {
                if (!_playerDataModel.IsGrounded)
                {
                    _spriteAnimator.StartAnimation(_playerView.SpriteRenderer, AnimationState.Jump, true,
                        AnimationSpeeds.NORMAL_ANIMATION_SPEED);
                }
                else
                {
                    if (isWalking)
                    {
                        _spriteAnimator.StartAnimation(_playerView.SpriteRenderer, AnimationState.Run, true,
                            AnimationSpeeds.NORMAL_ANIMATION_SPEED);
                    }
                    else
                    {
                        _spriteAnimator.StartAnimation(_playerView.SpriteRenderer, AnimationState.Idle, true,
                            AnimationSpeeds.NORMAL_ANIMATION_SPEED);
                    }
                }
            }

            _spriteAnimator.Execute(deltaTime);
        }

        public void Cleanup()
        {
            _playerLifeController.Cleanup();
            _playerTeleportController.Cleanup();
        }
    }
}