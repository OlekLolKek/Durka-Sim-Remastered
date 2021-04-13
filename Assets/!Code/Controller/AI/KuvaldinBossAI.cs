using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace DurkaSimRemastered
{
    public class KuvaldinBossAI : IInitialize, IExecute, ICleanup
    {
        private PlayerView _playerView;
        private EntityStates _state = EntityStates.Idle;
        private int _currentHealth;
        private bool _isDead;
        private bool _isJumping;
        private bool _seesPlayer;
        private bool _attacked;

        private readonly PlayerDataModel _kuvaldinDataModel;
        private readonly ContactPoller _contactPoller;
        private readonly KuvaldinBossView _view;
        private readonly SpriteAnimator _spriteAnimator;
        private readonly Transform _target;
        private readonly AIConfig _aiConfig;

        private readonly Vector3 _rightScale = new Vector3(1.0f, 1.0f);
        private readonly Vector3 _leftScale = new Vector3(-1.0f, 1.0f);

        private readonly float _jumpForce;
        
        private const float VISION_LENGTH = 10.0f;
        private const float DEFAULT_JUMP_FORCE = 350.0f;
        
        private const float CHARGE_TIME = 2.0f;
        private float _chargeTimer;

        private const float TRANSITION_TO_CHARGE_TIME = 0.5f;
        private float _transitionToChargeTimer;
        private bool _readyToCharge;

        public KuvaldinBossAI(KuvaldinBossView view, 
            SpriteAnimatorConfig kuvaldinAnimatorConfig,
            Transform target, AIConfig aiConfig)
        {
            _view = view;
            _target = target;
            _aiConfig = aiConfig;
            _kuvaldinDataModel = new PlayerDataModel();
            _contactPoller = new ContactPoller(_view.Collider2D, _kuvaldinDataModel);
            _spriteAnimator = new SpriteAnimator(kuvaldinAnimatorConfig);

            _currentHealth = _aiConfig.Health;
            _view.OnDamageReceived += OnDamageReceived;
            _jumpForce = DEFAULT_JUMP_FORCE * _view.Rigidbody2D.mass;
            _view.OnKuvaldinCollision += OnKuvaldinCollision;
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

        private void OnKuvaldinCollision(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out _playerView))
            {
                _playerView.Damage(_aiConfig.Damage);
            }
        }

        private void Die()
        {
            _isDead = true;
            _spriteAnimator.StopAnimation(_view.SpriteRenderer);
            _view.DeathParticleSystem.transform.SetParent(null);
            _view.DeathParticleSystem.Play();
            _view.gameObject.SetActive(false);
            _view.OnDamageReceived -= OnDamageReceived;
            _view.OnKuvaldinCollision -= OnKuvaldinCollision;
        }

        public void Initialize()
        {
            SetToIdle();
        }
        
        public void Execute(float deltaTime)
        {
            if (_isDead)
            {
                return;
            }
            
            _contactPoller.Execute(deltaTime);
            RaycastToPlayer(deltaTime);
            _spriteAnimator.Execute(deltaTime);
            Rotation();
            ProcessChargeTimer(deltaTime);
        }

        private void Rotation()
        {
            if (!_seesPlayer) return;
            
            if (_view.transform.position.x < _target.position.x)
            {
                _view.transform.localScale = _rightScale;
            }
            else
            {
                _view.transform.localScale = _leftScale;
            }
        }

        private void ProcessChargeTimer(float deltaTime)
        {
            if (!_readyToCharge)
            {
                _transitionToChargeTimer += deltaTime;
                if (_transitionToChargeTimer >= TRANSITION_TO_CHARGE_TIME)
                {
                    _transitionToChargeTimer = 0.0f;
                    _readyToCharge = true;
                }
            }
        }

        private void RaycastToPlayer(float deltaTime)
        {
            var position = _view.transform.position;
            position.y += _aiConfig.EnemyHeightOffset;
            var target = _target.position;
            target.y += _aiConfig.PlayerHeightOffset;
            var direction = target - position;
            var hit = Physics2D.Raycast(
                position, direction, 
                VISION_LENGTH, _aiConfig.LayerMask);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out _playerView))
                {
                    _seesPlayer = true;
                    if (_state == EntityStates.Idle || _state == EntityStates.Moving)
                    {
                        SetToCharging();
                    }
                    else if (_state == EntityStates.Charging)
                    {
                        ProcessCharging(deltaTime);
                    }
                    else if (_state == EntityStates.Attacking)
                    {
                        ProcessAttack(deltaTime);
                    }
                }
                else
                {
                    SetToIdle();
                    _seesPlayer = false;
                }
            }
        }

        private void ProcessCharging(float deltaTime)
        {
            if (!_seesPlayer)
            {
                SetToIdle();
            }
            
            if (_kuvaldinDataModel.IsGrounded)
            {
                _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: 0.0f);
                _chargeTimer += deltaTime;
                if (_chargeTimer >= CHARGE_TIME)
                {
                    SetToAttacking();
                }
            }
        }

        private void ProcessAttack(float deltaTime)
        {
            if (_kuvaldinDataModel.IsGrounded)
            {
                if (!_attacked)
                {
                    _attacked = true;
                    _readyToCharge = false;
                    _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: 0.0f);
                    _view.Rigidbody2D.AddForce(Vector2.up * _jumpForce);
                }
                else
                {
                    if (_readyToCharge)
                    {
                        SetToCharging();
                    }
                }
            }
            else
            {
                if (_target.position.x > _view.transform.position.x)
                {
                    _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: _aiConfig.Speed);
                }
                else
                {
                    _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: -_aiConfig.Speed);
                }
            }
        }

        private void SetToIdle()
        {
            _state = EntityStates.Idle;
            _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: 0.0f);
            _spriteAnimator.StartAnimation(
                _view.SpriteRenderer, AnimationState.Idle, 
                true, AnimationSpeeds.HALF_ANIMATION_SPEED);
        }
        
        private void SetToCharging()
        {
            _readyToCharge = false;
            _state = EntityStates.Charging;
            _view.Rigidbody2D.velocity = _view.Rigidbody2D.velocity.Change(x: 0.0f);
            _spriteAnimator.StartAnimation(
                _view.SpriteRenderer, AnimationState.Charge,
                false, AnimationSpeeds.NORMAL_ANIMATION_SPEED);
        }
        
        private void SetToAttacking()
        {
            _chargeTimer = 0.0f;
            _state = EntityStates.Attacking;
            _spriteAnimator.StartAnimation(
                _view.SpriteRenderer, AnimationState.Attack,
                false, AnimationSpeeds.NORMAL_ANIMATION_SPEED);
            _attacked = false;
        }

        public void Cleanup()
        {
            _view.OnDamageReceived -= OnDamageReceived;
            _view.OnKuvaldinCollision -= OnKuvaldinCollision;
        }
    }
}