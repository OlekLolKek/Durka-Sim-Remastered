using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class LemonLaser : IExecute
    {
        private readonly LemonLaserView _view;
        private readonly BulletEffectView _bulletEffectView;
        
        private readonly int _damage;
        private readonly float _throwForce;

        public LemonLaser(LemonLaserView view, BulletEffectView bulletEffectView,
            BulletConfig config)
        {
            _view = view;
            _bulletEffectView = bulletEffectView;
            _view.SetVisible(false);
            _view.gameObject.SetActive(false);

            _throwForce = config.ThrowForce;
            _damage = config.Damage;
        }

        public void Throw(Vector3 position, Vector2 velocity)
        {
            Debug.Log("pew");
            _view.gameObject.SetActive(true);
            _view.SetVisible(false);
            _view.transform.position = position;
            _view.Rigidbody2D.velocity = Vector2.zero;
            _view.Rigidbody2D.angularVelocity = 0.0f;
            _view.Rigidbody2D.AddForce(velocity * _throwForce, ForceMode2D.Impulse);
            _view.OnLaserCollision += OnLaserHit;
            _view.SetVisible(true);
        }

        private void OnLaserHit(Collision2D other)
        {
            _view.OnLaserCollision -= OnLaserHit;
            _bulletEffectView.transform.position = _view.transform.position;
            _bulletEffectView.Play();
            _view.gameObject.SetActive(false);

            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(_damage);
            }
        }
        
        public void Execute(float deltaTime)
        {
            RotateBullet();
        }

        private void RotateBullet()
        {
            Vector3 velocity = _view.Rigidbody2D.velocity;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            _view.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle - 90.0f);
            Debug.Log(_view.transform.rotation);
        }
    }
}