using UnityEngine;


namespace DurkaSimRemastered
{
    public class ElevatorView : MonoBehaviour
    {
        [SerializeField] private float _maxHeight;
        [SerializeField] private float _minHeight;

        public float MAXHeight => _maxHeight;
        public float MINHeight => _minHeight;
    }
}