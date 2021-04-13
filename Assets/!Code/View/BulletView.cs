using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class BulletView : LevelObjectView
    {
        public event Action<Collision2D> OnBulletCollision = delegate(Collision2D collision2D) {  }; 
        
        [SerializeField] private TrailRenderer _trail;

        public void SetVisible(bool visible)
        {
            if (_trail)
            {
                _trail.enabled = visible;
            }

            if (_trail)
            {
                _trail.Clear();
            }

            SpriteRenderer.enabled = visible;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            OnBulletCollision.Invoke(other);
        }
    }
}