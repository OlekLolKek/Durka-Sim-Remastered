using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class CameraController : IUpdate
    {
        private Vector3 _velocity;
        private readonly Transform _camera;
        private readonly Transform _player;
        private readonly float _offsetY = 0.75f;
        private readonly float _offsetZ = -10;
        private readonly float _smoothFactor = 0.15f;
        
        public CameraController(Transform camera, Transform player)
        {
            _camera = camera;
            _player = player;
        }


        public void Update()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            var targetPosition = _player.position;
            targetPosition.y += _offsetY;
            var newPosition = Vector3.SmoothDamp(_camera.position, targetPosition, ref _velocity, _smoothFactor);
            newPosition.z = _offsetZ;
            _camera.position = newPosition;
        }
    }
}