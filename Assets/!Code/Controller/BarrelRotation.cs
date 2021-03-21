using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class BarrelRotation : IUpdate
    {
        private readonly Camera _camera;
        private readonly Transform _playerTransform;
        private readonly Transform _barrelTransform;
        private Vector3 _direction = Vector3.zero;

        public BarrelRotation(Transform barrelTransform, Camera camera, Transform player)
        {
            _barrelTransform = barrelTransform;
            _camera = camera;
            _playerTransform = player;
        }

        public void Update()
        {
            var newPosition = _playerTransform.position;
            newPosition.y += 0.5f;
            _barrelTransform.position = newPosition;
            
            var mousePosition = Input.mousePosition;
            var barrelPosition = _camera.WorldToScreenPoint(_barrelTransform.position);
            mousePosition = mousePosition - barrelPosition;

            var angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

            angle = Mathf.Clamp(angle, -180, 180);
            _direction.z = angle;
            _barrelTransform.rotation = Quaternion.Euler(_direction);
        }
    }
}