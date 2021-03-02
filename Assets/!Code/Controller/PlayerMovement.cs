using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class PlayerMovement : IFixedUpdate
    {
        private readonly SpriteAnimator _spriteAnimator;
        private readonly ContactPoller _contactPoller;
        private readonly LevelObjectView _view;

        private readonly Vector3 _leftScale = new Vector3(-1.0f, 1.0f, 1.0f);
        private readonly Vector3 _rightScale = new Vector3(1.0f, 1.0f, 1.0f);

        private const float WALK_SPEED = 150.0f;
        private const float ANIMATIONS_SPEED = 10.0f;
        private const float JUMP_FORCE = 350.0f;
        private const float MOVING_THRESHOLD = 0.1f;
        private const float JUMP_THRESHOLD = 0.1f;
        private const float FLY_THRESHOLD = 1.0f;
        
        private bool _doJump = false;
        private float _goSideways = 0.0f;

        public PlayerMovement(LevelObjectView view, SpriteAnimator spriteAnimator)
        {
            _view = view;
            _spriteAnimator = spriteAnimator;
            _contactPoller = new ContactPoller(_view.Collider2D);
        }

        public void FixedUpdate()
        {
            _doJump = Input.GetAxis(AxisNames.VERTICAL) > 0;
            _goSideways = Input.GetAxis(AxisNames.HORIZONTAL);
            _contactPoller.Update();

            var isWalking = Mathf.Abs(_goSideways) > MOVING_THRESHOLD;
            
            
            //TODO: change to scale shifting
            if (isWalking) _view.SpriteRenderer.flipX = _goSideways < 0;
            var newVelocity = 0.0f;
            if (isWalking
                && (_goSideways > 0 || !_contactPoller.HasLeftContacts)
                && (_goSideways < 0 || !_contactPoller.HasRightContacts))
            {
                newVelocity = Time.fixedDeltaTime * WALK_SPEED * (_goSideways < 0 ? -1 : 1);
            }

            _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: newVelocity);
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
        }
    }
}