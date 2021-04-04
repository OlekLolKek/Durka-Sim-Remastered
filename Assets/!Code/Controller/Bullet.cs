using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public sealed class Bullet : IExecute
    {
        private readonly BulletView _view;
        private readonly BulletParticleSystemView _bulletParticleSystemView;

        public Bullet(BulletView view, BulletParticleSystemView bulletParticleSystemView)
        {
            _view = view;
            _bulletParticleSystemView = bulletParticleSystemView;
            _view.SetVisible(false);
            _view.gameObject.SetActive(false);
        }

        public void Throw(Vector3 position, Vector3 velocity)
        {
            _view.gameObject.SetActive(true);
            _view.SetVisible(false);
            _view.transform.position = position;
            _view.Rigidbody2D.velocity = Vector2.zero;
            _view.Rigidbody2D.angularVelocity = 0.0f;
            _view.Rigidbody2D.AddForce(velocity, ForceMode2D.Impulse);
            _view.OnBulletCollision += OnBulletHit;
            _view.SetVisible(true);
        }

        private void OnBulletHit(Collision2D collision)
        {
            _view.OnBulletCollision -= OnBulletHit;
            _bulletParticleSystemView.transform.position = _view.transform.position;
            _bulletParticleSystemView.Play();
            _view.gameObject.SetActive(false);
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
        }
    }
}