using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class AimingMuzzle : IUpdate
    {
        private readonly Camera _camera;
        private readonly Transform _barrelTransform;
        private Vector3 _direction = Vector3.zero;

        public AimingMuzzle(Transform barrelTransform, Camera camera)
        {
            _barrelTransform = barrelTransform;
            _camera = camera;
        }

        public void Update()
        {
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