using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class BulletsEmitter : IUpdate
    {
        private const float DELAY = 1.0f;
        private const float START_SPEED = 5.0f;

        private List<Bullet> _bullets = new List<Bullet>();
        private Transform _transform;

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

        public void Update()
        {
            if (_timeUntilNextBullet > 0)
            {
                _timeUntilNextBullet -= Time.deltaTime;
            }
            else
            {
                _timeUntilNextBullet = DELAY;
                _bullets[_currentIndex].Throw(_transform.position, _transform.up * START_SPEED);
                _currentIndex++;
                if (_currentIndex >= _bullets.Count)
                    _currentIndex = 0;
            }
            _bullets.ForEach(b => b.Update());
        }
    }
}