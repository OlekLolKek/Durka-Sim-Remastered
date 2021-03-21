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
        private const float ANIMATIONS_SPEED = 10.0f;
        private const float JUMP_FORCE = 400.0f;
        private const float MOVING_THRESHOLD = 0.1f;
        private const float JUMP_THRESHOLD = 0.1f;
        private const float FLY_THRESHOLD = 1.0f;
        
        private bool _doJump = false;
        private float _horizontal = 0.0f;

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
            
            _contactPoller.Execute(Time.fixedDeltaTime); //For some reason the Update method doesn't work correctly with Time.deltaTime, the velocity changes for some reason
                                                         //Time.fixedDeltaTime works just fine, even though it shouldn't
                                                         //Unity is fun

            var isWalking = Mathf.Abs(_horizontal) > MOVING_THRESHOLD;
            
            //TODO: change to scale shifting
            if (isWalking) _view.SpriteRenderer.flipX = _horizontal < 0;
            var newVelocity = 0.0f;
            if (isWalking
                && (_horizontal > 0 || !_contactPoller.HasLeftContacts)
                && (_horizontal < 0 || !_contactPoller.HasRightContacts))
            {
                newVelocity = Time.fixedDeltaTime * _horizontal * WALK_SPEED;
            }

            _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: newVelocity);

            Debug.Log($"{Time.time}");
            if (_contactPoller.IsGrounded && _doJump &&
                Mathf.Abs(_view.Rigidbody2D.velocity.y) <= JUMP_THRESHOLD)
            {
                _view.Rigidbody2D.AddForce(Vector3.up * JUMP_FORCE);
            }

            if (_contactPoller.IsGrounded)
            {
                var track = isWalking ? AnimationState.Run : AnimationState.Idle;
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, track, true, ANIMATIONS_SPEED);
            }
            else if (Mathf.Abs(_view.Rigidbody2D.velocity.y) > FLY_THRESHOLD)
            {
                var track = AnimationState.Jump;
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, track, true, ANIMATIONS_SPEED);
            }

            _spriteAnimator.Execute(deltaTime);
        }
    }
}