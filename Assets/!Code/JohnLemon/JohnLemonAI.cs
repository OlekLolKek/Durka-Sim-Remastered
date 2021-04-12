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
        private const float BURST_DELAY = 0.33f;
        private const float TIME_BETWEEN_BURSTS = 10.0f;
        private const int BURST_LASER_AMOUNT = 5;

        private int _currentHealth;

        private IDisposable _burstCoroutine;

        public JohnLemonAI(List<LemonLaserView> lemonLaserViews,
            List<LemonLaserView> burstLaserViews,
            List<BulletEffectView> bulletEffectViews,
            List<BulletEffectView> burstLasersEffectViews,
            Transform leftBulletSource, Transform rightBulletSource, 
            Transform player,
            BulletConfig laserConfig,
            BulletConfig burstLaserConfig,
            AIConfig johnLemonAIConfig)
        {
            _leftBulletSource = leftBulletSource;
            _rightBulletSource = rightBulletSource;
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

            _barrelRotation = new LemonBarrelRotation(_leftBulletSource, _rightBulletSource, player);
            
            _view = Object.FindObjectOfType<JohnLemonView>();

            _currentHealth = johnLemonAIConfig.Health;
        }
        
        public void Execute(float deltaTime)
        {
            _barrelRotation.Execute(deltaTime);

            CheckShooting(deltaTime);

            foreach (var laser in _lasers)
            {
                laser.Execute(deltaTime);
            }

            foreach (var burstLaser in _burstLasers)
            {
                burstLaser.Execute(deltaTime);
            }
        }

        private void CheckShooting(float deltaTime)
        {
            // if (_readyToBurst)
            // {
            //     _readyToFire = false;
            //     _burstCoroutine = StartBurst().ToObservable().Subscribe();
            // }
            // else
            // {
            //     if (_timeUntilNextBurst > 0)
            //     {
            //         _timeUntilNextBurst -= deltaTime;
            //     }
            //     else
            //     {
            //         _readyToBurst = true;
            //         _timeUntilNextBurst = TIME_BETWEEN_BURSTS;
            //     }
                
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
            //}
        }

        private IEnumerator StartBurst()
        {
            for (int i = 0; i < BURST_LASER_AMOUNT; i++)
            {
                _lasers[i].Throw(_leftBulletSource.position, _leftBulletSource.right);
                SwitchActiveBarrel();
                yield return new WaitForSeconds(BURST_DELAY);
            }

            _readyToBurst = false;
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
        }
    }
}