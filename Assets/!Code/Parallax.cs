using UnityEngine;


namespace DurkaSimRemastered
{
    public class Parallax
    {
        private readonly Transform _camera;
        private readonly Transform _background;
        private readonly Vector3 _cameraStartPosition;
        private readonly Vector3 _backgroundStartPosition;
        private const float RATIO = 0.3f;

        public Parallax(Transform camera, Transform background)
        {
            _camera = camera;
            _background = background;
            _cameraStartPosition = _camera.transform.position;
            _backgroundStartPosition = _background.transform.position;
        }

        public void Update()
        {
            _background.position = _backgroundStartPosition + (_camera.position - _cameraStartPosition) * RATIO;
        }
    }
}