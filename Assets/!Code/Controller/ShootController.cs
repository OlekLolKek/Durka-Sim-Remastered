using System;
using System.Collections;
using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Model;
using UniRx;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class ShootController : IExecute, ICleanup
    {
        private readonly BarrelRotation _barrelRotation;
        
        private readonly List<Bullet> _bullets = new List<Bullet>();
        private readonly PlayerView _playerView;
        private readonly Transform _bulletSource;
        private readonly InputModel _inputModel;
        private readonly AmmoModel _ammoModel;
        private readonly PlayerDataModel _playerDataModel;
        private readonly SpriteAnimator _spriteAnimator;

        private int _currentIndex;
        private float _timeUntilNextBullet;
        private bool _readyToFire = true;
        
        private IDisposable _coroutine;
        
        private const float SHOOTING_DELAY = 0.75f;

        public ShootController(List<BulletView> bulletViews, 
            List<BulletEffectView> bulletParticleSystemViews,
            Transform bulletSource, InputModel inputModel, AmmoModel ammoModel,
            BulletConfig bulletConfig, PlayerDataModel playerDataModel,
            Camera camera, PlayerView playerView,
            SpriteAnimator spriteAnimator)
        {
            _bulletSource = bulletSource;
            _inputModel = inputModel;
            
            for (var i = 0; i < bulletViews.Count; i++)
            {
                var bulletView = bulletViews[i];
                var bulletParticleSystemView = bulletParticleSystemViews[i];
                _bullets.Add(new Bullet(bulletView, bulletParticleSystemView, bulletConfig));
            }
            
            _barrelRotation = new BarrelRotation(_bulletSource, camera, playerView.transform,
                playerDataModel);

            _ammoModel = ammoModel;
            _playerDataModel = playerDataModel;
            _playerView = playerView;
            _spriteAnimator = spriteAnimator;

            _timeUntilNextBullet = SHOOTING_DELAY;
        }

        public void Execute(float deltaTime)
        {
            _barrelRotation.Execute(deltaTime);
            
            CheckShooting(deltaTime);

            foreach (var bullet in _bullets)
            {
                bullet.Execute(deltaTime);
            }
        }

        private void CheckShooting(float deltaTime)
        {
            if (_readyToFire)
            {
                if (_inputModel.GetFireButtonDown)
                {
                    if (_ammoModel.AmmoCount > 0)
                    {
                        _ammoModel.SetAmmoCount(_ammoModel.AmmoCount - 1);
                        _coroutine = StartShootingAnimation().ToObservable().Subscribe();
                        _currentIndex++;
                        if (_currentIndex >= _bullets.Count)
                        {
                            _currentIndex = 0;
                        }

                        _readyToFire = false;
                    }
                }
            }
            else
            {
                if (_timeUntilNextBullet > 0)
                {
                    _timeUntilNextBullet -= deltaTime;
                }
                else
                {
                    _readyToFire = true;
                    _playerDataModel.IsPlayerShooting = false;
                    _timeUntilNextBullet = SHOOTING_DELAY;
                }
            }
        }

        private IEnumerator StartShootingAnimation()
        {
            _playerDataModel.IsPlayerShooting = true;
            _spriteAnimator.StartAnimation(_playerView.SpriteRenderer, AnimationState.Attack, false, AnimationSpeeds.NORMAL_ANIMATION_SPEED);
            
            yield return new WaitForSeconds(0.4f);
            
            _bullets[_currentIndex].Throw(_bulletSource.position, _bulletSource.right);
            _playerDataModel.IsPlayerShooting = false;
        }

        public void Cleanup()
        {
            _coroutine?.Dispose();
        }
    }
}