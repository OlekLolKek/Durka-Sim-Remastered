using UnityEngine;


namespace DurkaSimRemastered
{
    public class ElevatorView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _maxHeight;
        [SerializeField] private float _minHeight;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public float MAXHeight => _maxHeight;
        public float MINHeight => _minHeight;
    }
}