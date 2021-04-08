using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class BarrelRotation : IExecute
    {
        private Vector3 _direction = Vector3.zero;

        private readonly PlayerDataModel _playerDataModel;
        private readonly Transform _playerTransform;
        private readonly Transform _barrelTransform;
        private readonly Camera _camera;

        private readonly Vector3 _barrelRightPositionOffset = new Vector2(0.25f, 0.85f);
        private readonly Vector3 _barrelLeftPositionOffset = new Vector2(-0.25f, 0.85f);

        public BarrelRotation(Transform barrelTransform, Camera camera, Transform player,
            PlayerDataModel playerDataModel)
        {
            _barrelTransform = barrelTransform;
            _camera = camera;
            _playerTransform = player;
            _playerDataModel = playerDataModel;
        }

        public void Execute(float deltaTime)
        {
            var offset = _playerDataModel.IsPlayerFacingRight
                ? _barrelRightPositionOffset
                : _barrelLeftPositionOffset;
            
            var newPosition = _playerTransform.position;
            newPosition += offset;
            _barrelTransform.position = newPosition;
            
            var mousePosition = Input.mousePosition;
            var barrelPosition = _camera.WorldToScreenPoint(_barrelTransform.position);
            mousePosition -= barrelPosition;

            var angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

            _direction.z = angle;
            _barrelTransform.rotation = Quaternion.Euler(_direction);
        }
    }
}