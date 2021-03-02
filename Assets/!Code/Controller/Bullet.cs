using UnityEngine;


namespace DurkaSimRemastered
{
    public class Bullet
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
    }
}