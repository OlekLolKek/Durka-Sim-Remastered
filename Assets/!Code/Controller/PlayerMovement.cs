using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class PlayerMovement : IExecute
    {
        private readonly SpriteAnimator _spriteAnimator;
        private readonly ContactPoller _contactPoller;
        private readonly LevelObjectView _view;
        private readonly InputModel _inputModel;

        private readonly Vector3 _leftScale = new Vector3(-1.0f, 1.0f, 1.0f);
        private readonly Vector3 _rightScale = new Vector3(1.0f, 1.0f, 1.0f);

        private const float WALK_SPEED = 150.0f;
        private const float JUMP_FORCE = 400.0f;
        private const float MOVING_THRESHOLD = 0.1f;
        private const float JUMP_THRESHOLD = 0.1f;
        private const float FALL_THRESHOLD = -0.1f;
        private const float FALL_TIME = 0.33f;
        private const float ELEVATOR_FALL_TIME = 0.33f;

        private float _fallTimer;
        private bool _doJump;
        private float _horizontal;
        private float _vertical;

        public PlayerMovement(LevelObjectView view, SpriteAnimatorConfig playerConfig,
            InputModel inputModel)
        {
            _view = view;
            _inputModel = inputModel;
            _spriteAnimator = new SpriteAnimator(playerConfig);
            _contactPoller = new ContactPoller(_view.Collider2D);
        }
        
        public void Execute(float deltaTime)
        {
            _doJump = _inputModel.GetJumpButtonDown;
            _horizontal = _inputModel.Horizontal;
            _vertical = _inputModel.Vertical;
            
            _contactPoller.Execute(Time.fixedDeltaTime); 
            
            //For some reason the Update method doesn't work correctly with Time.deltaTime, the velocity changes for some reason
            //Time.fixedDeltaTime works just fine, even though it shouldn't

            var isWalking = Mathf.Abs(_horizontal) > MOVING_THRESHOLD;
            
            Walk(isWalking);

            Jump();

            Animate(isWalking, deltaTime);

            Fall(deltaTime);
        }

        private void Walk(bool isWalking)
        {
            var newVelocity = 0.0f;
            
            if (isWalking)
            {
                if (_horizontal < 0)
                {
                    _view.transform.localScale = _leftScale;
                }
                else
                {
                    _view.transform.localScale = _rightScale;
                }
                
                if ((_horizontal > 0 && !_contactPoller.HasRightContacts)
                    || (_horizontal < 0 && !_contactPoller.HasLeftContacts))
                {
                    newVelocity = Time.fixedDeltaTime * _horizontal * WALK_SPEED;
                }
            }

            _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: newVelocity);
        }

        private void Jump()
        {
            if (_contactPoller.IsGrounded && _doJump &&
                Mathf.Abs(_view.Rigidbody2D.velocity.y - 
                          _contactPoller.GroundVelocity.y) <= JUMP_THRESHOLD)
            {
                _view.Rigidbody2D.AddForce(Vector3.up * JUMP_FORCE);
            }
        }

        private void Animate(bool isWalking, float deltaTime)
        {
            if (!_contactPoller.IsGrounded)
            {
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Jump, true, AnimationSpeeds.NORMAL_ANIMATION_SPEED);
            }
            else
            {
                if (isWalking)
                {
                    _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Run, true, AnimationSpeeds.NORMAL_ANIMATION_SPEED);
                }
                else
                {
                    _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Idle, true, AnimationSpeeds.NORMAL_ANIMATION_SPEED);
                }
            }

            _spriteAnimator.Execute(deltaTime);
        }

        private void Fall(float deltaTime)
        {
            if (Physics2D.GetIgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.ELEVATOR_LAYER)
            || Physics2D.GetIgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.PLATFORM_LAYER))
            {
                _fallTimer -= deltaTime;
                if (_fallTimer <= 0.0f)
                {
                    Physics2D.IgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.ELEVATOR_LAYER, false);
                    Physics2D.IgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.PLATFORM_LAYER, false);
                }
            }
            if (_contactPoller.IsStandingOnPlatform)
            {
                if (_vertical < FALL_THRESHOLD)
                {
                    _fallTimer = FALL_TIME;
                    Physics2D.IgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.PLATFORM_LAYER);
                }
            }

            if (_contactPoller.IsStandingOnElevator)
            {
                if (_vertical < FALL_THRESHOLD)
                {
                    _fallTimer = ELEVATOR_FALL_TIME;
                    Physics2D.IgnoreLayerCollision(LayerID.PLAYER_LAYER, LayerID.ELEVATOR_LAYER);
                }
            }
        }
    }
}