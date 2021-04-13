using System;
using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Pathfinding;
using UnityEngine;


namespace DurkaSimRemastered
{
    //TODO: refactor this class, divide by several smaller classes
    public class StalkerAI : IExecute, IFixedExecute, ICleanup
    {
        private PlayerView _playerView;

        private EntityStates _state;
        
        private float _recalculatePathTimer;
        private float _attackTimer;
        
        private readonly float _rotationSpeed;
        
        private readonly SpriteAnimator _spriteAnimator;
        private readonly EnemyView _view;
        private readonly StalkerAIModel _model;
        private readonly Transform _target;
        private readonly AIConfig _config;
        private readonly Seeker _seeker;

        private const float ATTACK_SQR_DISTANCE = 1.75f;
        private const float MOVING_SQR_VELOCITY = 0.5f;
        private const float RECALCULATE_PATH_FREQUENCY = 0.5f;
        private const float ATTACK_FREQUENCY = 1.0f;
        private const float SPRITE_ROTATION_OFFSET = -90.0f;
        private const float ANIMATION_SPEED = 5.0f;
        
        private int _currentHealth;

        private bool _isReadyToAttack;
        private bool _isReadyToRecalculatePath;
        private bool _rotateTowardsPlayer;
        
        private bool _isDead;

        public StalkerAI(EnemyView view, AIConfig config, 
            Seeker seeker, Transform target, SpriteAnimatorConfig animatorConfig)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _config = config != null ? config : throw new ArgumentNullException(nameof(config));
            _seeker = seeker != null ? seeker : throw new ArgumentNullException(nameof(seeker));
            _target = target != null ? target : throw new ArgumentNullException(nameof(target));

            _view.OnDamageReceived += OnDamageReceived;

            _spriteAnimator = new SpriteAnimator(animatorConfig);

            _model = new StalkerAIModel(_config);
            _rotationSpeed = _config.RotationSpeed;
            
            _currentHealth = _config.Health;
        }

        #region Methods

        public void Execute(float deltaTime)
        {
            if (_isDead) return;
            
            _spriteAnimator.Execute(deltaTime);
            ProcessTimings(deltaTime);

            switch (_state)
            {
                case EntityStates.Idle:
                    RecalculatePath();
                    break;
                case EntityStates.Moving:
                    RecalculatePath();
                    break;
                case EntityStates.Attacking:
                    RecalculatePath();
                    Attack();
                    break;
            }
        }
        
        public void FixedExecute()
        {
            if (_isDead) return;
            
            var position = _view.transform.position;
            
            var newVelocity = _model.CalculateVelocity(position) * Time.fixedDeltaTime;
            Vector3 direction;
            if (_rotateTowardsPlayer)
            {
                var target = _target.position;
                target.y += _config.PlayerHeightOffset;
                direction = target - position;
            }
            else
            {
                direction = _model.CalculateDirection(position);
            }
            _view.Rigidbody2D.velocity = newVelocity;
            
            Rotate(direction);
            Animate();
        }

        private void ProcessTimings(float deltaTime)
        {
            if (!_isReadyToRecalculatePath)
            {
                _recalculatePathTimer += deltaTime;

                if (_recalculatePathTimer >= RECALCULATE_PATH_FREQUENCY)
                {
                    _recalculatePathTimer = 0.0f;
                    _isReadyToRecalculatePath = true;
                }
            }

            if (!_isReadyToAttack)
            {
                _attackTimer += deltaTime;
            
                if (_attackTimer >= ATTACK_FREQUENCY)
                {
                    _attackTimer = 0.0f;
                    _isReadyToAttack = true;
                }
            }
        }

        private void Rotate(Vector3 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle + SPRITE_ROTATION_OFFSET));
            _view.transform.rotation = Quaternion.Lerp(_view.transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        private void Animate()
        {
            switch (_state)
            {
                case EntityStates.Attacking:
                    _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Idle, true, ANIMATION_SPEED);
                    break;
                case EntityStates.Moving:
                    _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Run, true, ANIMATION_SPEED);
                    break;
                case EntityStates.Idle:
                    _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimationState.Idle, true, ANIMATION_SPEED);
                    break;
            }
        }

        private void RecalculatePath()
        {
            if (!_isReadyToRecalculatePath) return;
            
            _isReadyToRecalculatePath = false;
            
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
            var target = _target.position;
            target.y += _config.PlayerHeightOffset;
            var direction = target - position;
            var hit = Physics2D.Raycast(
                position, direction, 
                _config.VisibilityLength, _config.LayerMask);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out _playerView))
                {
                    if (direction.sqrMagnitude <= ATTACK_SQR_DISTANCE)
                    {
                        _state = EntityStates.Attacking;
                        _rotateTowardsPlayer = true;
                    }
                    else
                    {
                        _state = EntityStates.Moving;
                        _rotateTowardsPlayer = false;
                    }
                    
                    return true;
                }
                else if (_view.Rigidbody2D.velocity.sqrMagnitude >= MOVING_SQR_VELOCITY)
                {
                    _state = EntityStates.Moving;
                }
                else
                {
                    _state = EntityStates.Idle;
                }
                
            }

            return false;
        }

        private void Attack()
        {
            if (!_isReadyToAttack) return;
            _isReadyToAttack = false;
            _playerView.Damage(_config.Damage);
        }

        private void OnDamageReceived(int damage)
        {
            _currentHealth -= damage;
            _view.DamageParticleSystem.Play();
            _view.AudioSource.Play();
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _view.OnDamageReceived -= OnDamageReceived;
            _view.DeathParticleSystem.transform.SetParent(null); 
            _view.gameObject.SetActive(false);
            _view.DeathParticleSystem.Play();
            _isDead = true;
        }

        private void OnPathComplete(Path path)
        {
            if (path.error) return;
            _model.UpdatePath(path);
        }
        
        public void Cleanup()
        {
            _view.OnDamageReceived -= OnDamageReceived;
        }

        #endregion
    }
}