using UnityEngine;


namespace DurkaSimRemastered
{
    public class AmmoDropperView : MonoBehaviour
    {
        [SerializeField] private Vector3 _endPosition;

        public Vector3 EndPosition => _endPosition;
    }
}