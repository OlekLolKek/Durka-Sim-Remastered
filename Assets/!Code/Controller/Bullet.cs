using DurkaSimRemastered.Interface;
using Unity.Mathematics;
using UnityEngine;


namespace DurkaSimRemastered
{
    public sealed class Bullet : IExecute
    {
        private readonly BulletView _view;

        public Bullet(BulletView view)
        {
            _view = view;
            _view.SetVisible(false);
        }

        public void Throw(Vector3 position, Vector3 velocity)
        {
            _view.SetVisible(false);
            _view.transform.position = position;
            _view.Rigidbody2D.velocity = Vector2.zero;
            _view.Rigidbody2D.angularVelocity = 0.0f;
            _view.Rigidbody2D.AddForce(velocity, ForceMode2D.Impulse);
            _view.SetVisible(true);
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