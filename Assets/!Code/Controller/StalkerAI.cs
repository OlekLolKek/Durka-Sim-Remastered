using System;
using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Pathfinding;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class StalkerAI : IInitialize, IExecute, IFixedExecute
    {
        private List<RaycastHit2D> _raycastResults;
        
        private float _recalculatePathTimer;
        
        private readonly float _rotationSpeed;
        
        private readonly SpriteAnimator _spriteAnimator;
        private readonly LevelObjectView _view;
        private readonly StalkerAIModel _model;
        private readonly Transform _target;
        private readonly AIConfig _config;
        private readonly Seeker _seeker;

        private const float RECALCULATE_PATH_FREQUENCY = 0.5f;
        private const float SPRITE_ROTATION_OFFSET = -90.0f;
        private const float IDLE_VELOCITY_THRESHOLD = 0.1f;
        private const float ROTATION_THRESHOLD = 0.1f;
        private const float ANIMATION_SPEED = 5.0f;

        public StalkerAI(LevelObjectView view, AIConfig config, 
            Seeker seeker, Transform target, SpriteAnimatorConfig animatorConfig)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _config = config != null ? config : throw new ArgumentNullException(nameof(config));
            _seeker = seeker != null ? seeker : throw new ArgumentNullException(nameof(seeker));
            _target = target != null ? target : throw new ArgumentNullException(nameof(target));

            _spriteAnimator = new SpriteAnimator(animatorConfig);

            _model = new StalkerAIModel(_config);
            _rotationSpeed = _config.RotationSpeed;
        }

        #region Methods

        public void Initialize()
        {
            RecalculatePath();
        }

        public void Execute(float deltaTime)
        {
            _recalculatePathTimer += deltaTime;
            _spriteAnimator.Execute(deltaTime);

            if (_recalculatePathTimer >= RECALCULATE_PATH_FREQUENCY)
            {
                _recalculatePathTimer = 0.0f;

                RecalculatePath();
            }
        }

        public void FixedExecute()
        {
            var position = _view.transform.position;
            
            var newVelocity = _model.CalculateVelocity(position) * Time.fixedDeltaTime;
            var direction = _model.CalculateDirection(position);
            _view.Rigidbody2D.velocity = newVelocity;
            
            Rotate(direction);
            Animate(direction);
        }

        private void Rotate(Vector3 direction)
        {
            if (_view.Rigidbody2D.velocity.magnitude <= ROTATION_THRESHOLD)
                return;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + SPRITE_ROTATION_OFFSET));
            _view.transform.rotation = Quaternion.Lerp(_view.transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        private void Animate(Vector3 velocity)
        {
            if (velocity.magnitude < IDLE_VELOCITY_THRESHOLD)
            {
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Idle, true, ANIMATION_SPEED);
            }
            else
            {
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Run, true, ANIMATION_SPEED);
            }
        }

        private void RecalculatePath()
        {
            if (_seeker.IsDone())
            {
                if (CheckVisibility())
                {
                    var target = _target.position;
                    target.y += _config.PlayerHeightOffset;
                    _seeker.StartPath(_view.Rigidbody2D.position, target, OnPathComplete);
                }
            }
        }

        private bool CheckVisibility()
        {
            var position = _view.transform.position;
            var direction = _target.position - position;
            var hit = Physics2D.Raycast(position, direction, _config.VisibilityLength, _config.LayerMask);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out PlayerView playerView))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnPathComplete(Path path)
        {
            if (path.error) return;
            _model.UpdatePath(path);
        }

        #endregion
    }
}