using DurkaSimRemastered.Interface;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class ParallaxController : IExecute
    {
        private readonly Transform _camera;
        private readonly Transform _background;
        private readonly Vector3 _cameraStartPosition;
        private readonly Vector3 _backgroundStartPosition;
        private readonly float _ratio = 0.15f;

        public ParallaxController(Transform camera, Transform background, 
            float parallaxRatio)
        {
            _camera = camera;
            _background = background;
            _cameraStartPosition = _camera.transform.position;
            _backgroundStartPosition = _background.transform.position;
            _ratio = parallaxRatio;
        }

        public void Execute(float deltaTime)
        {
            _background.position = _backgroundStartPosition + (_camera.position - _cameraStartPosition) * _ratio;
        }
    }
}