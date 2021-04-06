using UnityEngine;


namespace DurkaSimRemastered
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "Configs/AI", order = 0)]
    public class AIConfig : ScriptableObject
    {
        [SerializeField] private int _health;
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _minSqrDistanceToTarget;
        [SerializeField] private float _visibilityLength;
        [SerializeField] private float _playerHeightOffset = 0.5f;
        [SerializeField] private LayerMask _layerMask;

        public int Health => _health;
        public int Damage => _damage;
        public float Speed => _speed;
        public float RotationSpeed => _rotationSpeed;
        public float MinSqrDistanceToTarget => _minSqrDistanceToTarget;
        public float VisibilityLength => _visibilityLength;
        public float PlayerHeightOffset => _playerHeightOffset;
        public LayerMask LayerMask => _layerMask;
    }
}