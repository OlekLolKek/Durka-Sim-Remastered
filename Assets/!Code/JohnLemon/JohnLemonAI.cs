using System;
using System.Collections;
using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public class JohnLemonAI : IExecute, ICleanup
    {
        private Transform _activeBulletSource;
        private readonly Transform _leftBulletSource;
        private readonly Transform _rightBulletSource;
        private readonly Transform _player;
        private readonly AIConfig _johnLemonAIConfig;
        private readonly JohnLemonLifeModel _johnLemonLifeModel;
        private readonly LemonBarrelRotation _barrelRotation;
        private readonly JohnLemonView _view;
        private readonly SpriteAnimator _spriteAnimator;
        private readonly List<LemonLaser> _lasers = new List<LemonLaser>();
        private readonly List<LemonLaser> _burstLasers = new List<LemonLaser>();

        private int _currentIndex;
        
        private bool _readyToFire;
        private float _timeUntilNextLaser;
        private const float SHOOTING_DELAY = 1.5f;

        private bool _readyToBurst;
        private float _timeUntilNextBurst;
        private const float BURST_DELAY = 0.1f;
        private const float TIME_BETWEEN_BURSTS = 10.0f;

        private bool _bursting;

        private IDisposable _burstCoroutine;

        public JohnLemonAI(List<LemonLaserView> lemonLaserViews,
            List<LemonLaserView> burstLaserViews,
            List<BulletEffectView> bulletEffectViews,
            List<BulletEffectView> burstLasersEffectViews,
            Transform leftBulletSource, Transform rightBulletSource, 
            Transform player,
            BulletConfig laserConfig,
            BulletConfig burstLaserConfig,
            AIConfig johnLemonAIConfig,
            JohnLemonLifeModel johnLemonLifeModel)
        {
            _leftBulletSource = leftBulletSource;
            _rightBulletSource = rightBulletSource;
            _player = player;
            _johnLemonAIConfig = johnLemonAIConfig;
            _johnLemonLifeModel = johnLemonLifeModel;
            _activeBulletSource = _leftBulletSource;

            for (int i = 0; i < lemonLaserViews.Count; i++)
            {
                var laserView = lemonLaserViews[i];
                var bulletEffectView = bulletEffectViews[i];
                _lasers.Add(new LemonLaser(laserView, bulletEffectView, laserConfig));
            }

            for (int i = 0; i < burstLaserViews.Count; i++)
            {
                var laserView = burstLaserViews[i];
                var effectView = burstLasersEffectViews[i];
                _burstLasers.Add(new LemonLaser(laserView, effectView, burstLaserConfig));
            }

            _timeUntilNextBurst = TIME_BETWEEN_BURSTS;

            _barrelRotation = new LemonBarrelRotation(_leftBulletSource, _rightBulletSource, player);
            
            _view = Object.FindObjectOfType<JohnLemonView>();
            
            _view.OnDamageReceived += OnDamageReceived;

            _johnLemonLifeModel.OnLemonDied += Die;
        }
        
        public void Execute(float deltaTime)
        {
            if (!_johnLemonLifeModel.IsDead)
            {
                _barrelRotation.Execute(deltaTime);

                if (CheckVisibility())
                {
                    CheckShooting(deltaTime);
                }
            }

            foreach (var laser in _lasers)
            {
                laser.Execute(deltaTime);
            }

            foreach (var burstLaser in _burstLasers)
            {
                burstLaser.Execute(deltaTime);
            }
        }

        private void OnDamageReceived(int damage)
        {
            _johnLemonLifeModel.SetHealth(_johnLemonLifeModel.CurrentHealth - damage);
            _view.DamageParticleSystem.Play();
        }

        private void Die()
        {
            _johnLemonLifeModel.OnLemonDied -= Die;
            _view.OnDamageReceived -= OnDamageReceived;
            _view.DamageParticleSystem.Play();
            _view.DeathParticleSystem.Play();
        }

        private bool CheckVisibility()
        {
            var position = _view.transform.position;
            position.y += _johnLemonAIConfig.EnemyHeightOffset;
            var target = _player.position;
            target.y += _johnLemonAIConfig.PlayerHeightOffset;
            var direction = target - position;
            var hit = Physics2D.Raycast(
                position, direction, 
                _johnLemonAIConfig.VisibilityLength, _johnLemonAIConfig.LayerMask);
            
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out PlayerView playerView))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void CheckShooting(float deltaTime)
        {
            if (_readyToBurst)
            {
                _readyToFire = false;
                _readyToBurst = false;
                _burstCoroutine = StartBurst().ToObservable().Subscribe();
            }
            else
            {
                if (_timeUntilNextBurst > 0)
                {
                    _timeUntilNextBurst -= deltaTime;
                }
                else
                {
                    _readyToBurst = true;
                    _timeUntilNextBurst = TIME_BETWEEN_BURSTS;
                }

                if (!_bursting)
                {
                    if (_readyToFire)
                    {
                        _lasers[_currentIndex].Throw(_activeBulletSource.position, _activeBulletSource.right);
                        SwitchActiveBarrel();
                        _currentIndex++;
                        if (_currentIndex >= _lasers.Count)
                        {
                            _currentIndex = 0;
                        }
                    
                        _readyToFire = false;
                    }
                    else
                    {
                        if (_timeUntilNextLaser > 0)
                        {
                            _timeUntilNextLaser -= deltaTime;
                        }
                        else
                        {
                            _readyToFire = true;
                            _timeUntilNextLaser = SHOOTING_DELAY;
                        }
                    }
                }
            }
        }

        private IEnumerator StartBurst()
        {
            _bursting = true;

            foreach (var burstLaser in _burstLasers)
            {
                burstLaser.Throw(_activeBulletSource.position, _activeBulletSource.right);
                SwitchActiveBarrel();
                yield return new WaitForSeconds(BURST_DELAY);
            }
            
            _bursting = false;
        }

        private void SwitchActiveBarrel()
        {
            if (_activeBulletSource == _leftBulletSource)
            {
                _activeBulletSource = _rightBulletSource;
            }
            else
            {
                _activeBulletSource = _leftBulletSource;
            }
        }

        public void Cleanup()
        {
            _burstCoroutine?.Dispose();
            _johnLemonLifeModel.OnLemonDied -= Die;
            _view.OnDamageReceived -= OnDamageReceived;
        }
    }
}