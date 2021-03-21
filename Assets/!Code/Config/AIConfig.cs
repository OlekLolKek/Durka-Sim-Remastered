using UnityEngine;


namespace DurkaSimRemastered
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "Configs/AI", order = 0)]
    public class AIConfig : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _minSqrDistanceToTarget;

        public float Speed => _speed;
        public float RotationSpeed => _rotationSpeed;
        public float MinSqrDistanceToTarget => _minSqrDistanceToTarget;
    }
}