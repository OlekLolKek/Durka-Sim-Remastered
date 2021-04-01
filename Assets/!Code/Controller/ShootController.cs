using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class ShootController : IExecute
    {
        private const float DELAY = 0.75f;
        private const float START_SPEED = 10.0f;

        private readonly List<Bullet> _bullets = new List<Bullet>();
        private readonly InputModel _inputModel;
        private readonly Transform _transform;

        private int _currentIndex;
        private float _timeUntilNextBullet;
        private bool _readyToFire = true;

        public ShootController(List<BulletView> bulletViews, List<BulletParticleSystemView> bulletParticleSystemViews,
            Transform transform, InputModel inputModel)
        {
            _transform = transform;
            _inputModel = inputModel;
            for (var i = 0; i < bulletViews.Count; i++)
            {
                var bulletView = bulletViews[i];
                var bulletParticleSystemView = bulletParticleSystemViews[i];
                _bullets.Add(new Bullet(bulletView, bulletParticleSystemView));
            }
        }

        public void Execute(float deltaTime)
        {
            if (_readyToFire)
            {
                if (_inputModel.GetFireButtonDown)
                {
                    _bullets[_currentIndex].Throw(_transform.position, _transform.right * START_SPEED);
                    _currentIndex++;
                    if (_currentIndex >= _bullets.Count)
                    {
                        _currentIndex = 0;
                    }

                    _readyToFire = false;
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
                    _timeUntilNextBullet = DELAY;
                }
            }
            
            foreach (var bullet in _bullets)
            {
                bullet.Execute(deltaTime);
            }
        }
    }
}