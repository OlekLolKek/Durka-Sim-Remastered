using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class CameraController : IExecute
    {
        private Vector3 _velocity;
        private readonly Transform _camera;
        private readonly Transform _player;
        private readonly float _offsetY = 1.0f;
        private readonly float _offsetZ = -10;

        public CameraController(Transform camera, Transform player)
        {
            _camera = camera;
            _player = player;
        }


        public void Execute(float deltaTime)
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            var targetPosition = _player.position;
            targetPosition.y += _offsetY;
            var newPosition = targetPosition;
            newPosition.z = _offsetZ;
            _camera.position = newPosition;
        }
    }
}