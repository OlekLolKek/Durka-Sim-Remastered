using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class Bullet : IUpdate
    {
        private readonly BulletView _view;
        private Vector3 _velocity;
        private Transform _muzzle;
        
        private float _radius = 0.3f;
        private float _groundLevel = -1.0f;
        private float _g = -10.0f;
        

        public Bullet(BulletView view)
        {
            _view = view;
            _view.SetVisible(false);
        }

        public void Update()
        {
            if (IsGrounded())
            {
                Debug.Log("NotGrounded");
                SetVelocity(_velocity.Change(y: -_velocity.y));
                _view.transform.position = _view.transform.position.Change(y: _groundLevel + _radius);
            }
            else
            {
                SetVelocity(_velocity + Vector3.up * (_g * Time.deltaTime));
                _view.transform.position += _velocity * Time.deltaTime;
            }
        }

        public void Throw(Transform parent, Vector3 velocity)
        {
            _muzzle = parent;
            _view.transform.SetParent(parent);
            _view.transform.localPosition = Vector3.zero;
            _view.transform.SetParent(null);
            
            SetVelocity(velocity);
            _view.SetVisible(true);
        }

        private void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
            var angle = Vector3.Angle(Vector3.left, _velocity);
            var axis = Vector3.Cross(Vector3.left, _velocity);
            _view.transform.rotation = Quaternion.AngleAxis(angle, axis);
        }
        
        private bool IsGrounded()
        {
            return _view.transform.position.y <= _groundLevel + _radius + float.Epsilon && _velocity.y <= 0;
        }
    }
}