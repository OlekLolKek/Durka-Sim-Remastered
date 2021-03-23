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
        
        private readonly SpriteAnimator _spriteAnimatorConfig;
        private readonly LevelObjectView _view;
        private readonly StalkerAIModel _model;
        private readonly AIConfig _config;
        private readonly Seeker _seeker;
        private readonly Transform _target;

        private readonly float _rotationSpeed;
        private const float IDLE_VELOCITY_THRESHOLD = 0.1f;
        private const float ANIMATION_SPEED = 10.0f;
        private const float RECALCULATE_PATH_FREQUENCY = 0.5f;
        private const float SPRITE_ROTATION_OFFSET = -90.0f;
        private float _recalculatePathTimer;

        public StalkerAI(LevelObjectView view, AIConfig config, 
            Seeker seeker, Transform target, SpriteAnimatorConfig animatorConfig)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _config = config != null ? config : throw new ArgumentNullException(nameof(config));
            _seeker = seeker != null ? seeker : throw new ArgumentNullException(nameof(seeker));
            _target = target != null ? target : throw new ArgumentNullException(nameof(target));

            _spriteAnimatorConfig = new SpriteAnimator(animatorConfig);

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
            _view.Rigidbody2D.velocity = newVelocity;
            
            Rotate();
            Animate();
        }

        private void Rotate()
        {
            if (_view.Rigidbody2D.velocity.magnitude <= 0.25f)
                return;
            
            Vector3 target = _view.Rigidbody2D.velocity;
            var direction = target;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + SPRITE_ROTATION_OFFSET));
            _view.transform.rotation = Quaternion.Slerp(_view.transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        private void Animate()
        {
            if (_view.Rigidbody2D.velocity.magnitude < IDLE_VELOCITY_THRESHOLD)
            {
                _spriteAnimatorConfig.StartAnimation(_view.SpriteRenderer, AnimationState.Idle, true, ANIMATION_SPEED);
            }
            else
            {
                _spriteAnimatorConfig.StartAnimation(_view.SpriteRenderer, AnimationState.Run, true, ANIMATION_SPEED);
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