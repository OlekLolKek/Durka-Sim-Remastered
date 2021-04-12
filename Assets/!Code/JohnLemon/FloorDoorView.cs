using DG.Tweening;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class FloorDoorView : MonoBehaviour
    {
        [SerializeField] private Transform _leftDoor;
        [SerializeField] private Transform _rightDoor;
        [SerializeField] private Vector3 _maxRightPosition;
        [SerializeField] private Vector3 _maxLeftPosition;
        [SerializeField] private float _tweenTime = 2.5f;

        public void Open()
        {
            _leftDoor.DOMove(_maxLeftPosition, _tweenTime);
            _rightDoor.DOMove(_maxRightPosition, _tweenTime);
        }
    }
}