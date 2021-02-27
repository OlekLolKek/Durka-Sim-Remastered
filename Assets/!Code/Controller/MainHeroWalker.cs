using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class MainHeroWalker : IUpdate
    {
        private readonly SpriteAnimator _spriteAnimator;
        private readonly LevelObjectView _view;
        private readonly Vector3 _leftScale = new Vector3(-1.0f, 1.0f, 1.0f);
        private readonly Vector3 _rightScale = new Vector3(1.0f, 1.0f, 1.0f);

        private const float WALK_SPEED = 3.0f;
        private const float ANIMATIONS_SPEED = 10.0f;
        private const float JUMP_START_SPEED = 8.0f;
        private const float MOVING_THRESHOLD = 0.1f;
        private const float FLY_THRESHOLD = 1.0f;
        private const float GROUND_LEVEL = -1.0f;
        private const float G = -10f;
        
        private float _yVelocity;
        private float _xAxisInput = 0;
        private bool _doJump = false;

        public MainHeroWalker(LevelObjectView view, SpriteAnimator spriteAnimator)
        {
            _view = view;
            _spriteAnimator = spriteAnimator;
        }

        public void Update()
        {
            _doJump = Input.GetAxis(AxisNames.VERTICAL) > 0;
            _xAxisInput = Input.GetAxis(AxisNames.HORIZONTAL);
            var goSideways = Mathf.Abs(_xAxisInput) > MOVING_THRESHOLD;

            if (IsGrounded())
            {
                if (goSideways)
                {
                    GoSideways();
                }
                
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, goSideways ? AnimationState.Run : AnimationState.Idle, true, ANIMATIONS_SPEED);
                if (_doJump && _yVelocity == 0)
                {
                    _yVelocity = JUMP_START_SPEED;
                }
                else if (_yVelocity < 0)
                {
                    _yVelocity = 0;
                    _view.transform.position = _view.transform.position.Change(y: GROUND_LEVEL);
                }
            }
            else
            {
                if (goSideways) GoSideways();
                if (Mathf.Abs(_yVelocity) > FLY_THRESHOLD)
                {
                    //TODO: fix after making the jump animations 
                    //_spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Jump, true, ANIMATIONS_SPEED);
                }

                _yVelocity += G * Time.deltaTime;
                _view.transform.position += Vector3.up * (Time.deltaTime * _yVelocity);
            }
        }

        private void GoSideways()
        {
            _view.transform.position += Vector3.right * (Time.deltaTime * WALK_SPEED * (_xAxisInput < 0 ? -1 : 1));
            _view.transform.localScale = (_xAxisInput < 0 ? _leftScale : _rightScale);
        }

        private bool IsGrounded()
        {
            return _view.transform.position.y <= GROUND_LEVEL + float.Epsilon && _yVelocity <= 0;
        }
    }
}