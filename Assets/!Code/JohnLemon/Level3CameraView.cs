using System;
using UnityEngine;
using UnityEngine.U2D;


namespace DurkaSimRemastered
{
    public class Level3CameraView : MonoBehaviour
    {
        [SerializeField] private float _minSize = 3.75f;
        [SerializeField] private float _maxSize = 7.5f;
        [SerializeField] private Vector2 _minResolution;
        [SerializeField] private Vector2 _maxResolution;
        [SerializeField] private Vector3 _centerPosition;
        [SerializeField] private PixelPerfectCamera _pixelPerfectCamera;
        [SerializeField] private Camera _camera;

        public float MINSize => _minSize;
        public float MAXSize => _maxSize;
        public Vector2 MINResolution => _minResolution;
        public Vector2 MAXResolution => _maxResolution;
        public Vector3 CenterPosition => _centerPosition;
        public PixelPerfectCamera PixelPerfectCamera => _pixelPerfectCamera;
        public Camera Camera => _camera;
    }
}