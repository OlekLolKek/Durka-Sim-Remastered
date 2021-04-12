using DurkaSimRemastered.Interface;
using Unity.Mathematics;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class LemonBarrelRotation : IExecute
    {
        private readonly Transform _playerTransform;
        private readonly Transform _leftBarrelTransform;
        private readonly Transform _rightBarrelTransform;

        private const float PLAYER_HEIGHT_OFFSET = 0.5f;
        private const float ROTATION_SPEED = 10.0f;

        public LemonBarrelRotation(Transform leftBarrelTransform, 
            Transform rightBarrelTransform,
            Transform playerTransform)
        {
            _leftBarrelTransform = leftBarrelTransform;
            _rightBarrelTransform = rightBarrelTransform;
            _playerTransform = playerTransform;
        }

        public void Execute(float deltaTime)
        {
            var position = _playerTransform.position;
            position.y += PLAYER_HEIGHT_OFFSET;
            
            Vector2 leftDirection = position - _leftBarrelTransform.position;
            var leftAngle = Mathf.Atan2(leftDirection.y, leftDirection.x) * Mathf.Rad2Deg;
            var leftRotation = Quaternion.AngleAxis(leftAngle, Vector3.forward);
            _leftBarrelTransform.rotation =
                Quaternion.Slerp(_leftBarrelTransform.rotation, leftRotation, ROTATION_SPEED * deltaTime);
            
            Vector2 rightDirection = position - _rightBarrelTransform.position;
            var rightAngle = Mathf.Atan2(rightDirection.y, rightDirection.x) * Mathf.Rad2Deg;
            var rightRotation = Quaternion.AngleAxis(rightAngle, Vector3.forward);
            _rightBarrelTransform.rotation =
                Quaternion.Slerp(_rightBarrelTransform.rotation, rightRotation, ROTATION_SPEED * deltaTime);
        }
    }
}