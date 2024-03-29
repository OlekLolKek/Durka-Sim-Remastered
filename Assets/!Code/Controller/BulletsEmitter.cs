using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class BulletsEmitter : IExecute
    {
        private const float DELAY = 1.0f;
        private const float START_SPEED = 10.0f;

        private readonly List<Bullet> _bullets = new List<Bullet>();
        private readonly Transform _transform;

        private int _currentIndex;
        private float _timeUntilNextBullet;

        public BulletsEmitter(List<BulletView> bulletViews, Transform transform)
        {
            _transform = transform;
            foreach (var bulletView in bulletViews)
            {
                _bullets.Add(new Bullet(bulletView));
            }
        }

        public void Execute(float deltaTime)
        {
            if (_timeUntilNextBullet > 0)
            {
                _timeUntilNextBullet -= deltaTime;
            }
            else
            {
                _timeUntilNextBullet = DELAY;
                _bullets[_currentIndex].Throw(_transform.position, _transform.right * START_SPEED);
                _currentIndex++;
                if (_currentIndex >= _bullets.Count)
                {
                    _currentIndex = 0;
                }
            }

            foreach (var bullet in _bullets)
            {
                bullet.Execute(deltaTime);
            }
        }
    }
}