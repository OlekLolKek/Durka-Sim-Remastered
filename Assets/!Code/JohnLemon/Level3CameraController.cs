using System;
using System.Collections;
using DG.Tweening;
using DurkaSimRemastered.Interface;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


namespace DurkaSimRemastered
{
    public class Level3CameraController : IExecute, ICleanup
    {
        private readonly Level3CameraView _cameraView;
        private readonly LevelObjectView _startTrigger;
        private readonly LevelObjectView _endTrigger;
        private readonly Transform _player;

        private const float CAMERA_MOVE_TIME = 1.5f;
        private const float OFFSET_Y = 1.0f;
        private const float OFFSET_Z = -10;

        private bool _followPlayer = true;
        private IDisposable _zoomOutCoroutine;
        private IDisposable _zoomInCoroutine;

        public Level3CameraController(LevelObjectView startTrigger,
            LevelObjectView endTrigger, Transform player)
        {
            _startTrigger = startTrigger;
            _endTrigger = endTrigger;
            _player = player;
            _cameraView = Object.FindObjectOfType<Level3CameraView>();

            _startTrigger.OnTriggerEnter += OnStartTriggerEnter;
            _endTrigger.OnTriggerEnter += OnEndTriggerEnter;
        }
        
        public void Execute(float deltaTime)
        {
            if (_followPlayer)
            {
                MoveCameraToPlayer();
            }
        }

        private IEnumerator CameraZoomOut()
        {
            _cameraView.PixelPerfectCamera.enabled = false;
            _cameraView.transform.DOMove(_cameraView.CenterPosition, CAMERA_MOVE_TIME);
            while (_cameraView.Camera.orthographicSize < _cameraView.MAXSize)
            {
                float newSize = _cameraView.Camera.orthographicSize;
                newSize += 0.05f;
                _cameraView.Camera.orthographicSize = newSize;
                yield return 0;
            }
            _cameraView.PixelPerfectCamera.enabled = true;
            _cameraView.PixelPerfectCamera.refResolutionX = (int) _cameraView.MAXResolution.x;
            _cameraView.PixelPerfectCamera.refResolutionY = (int) _cameraView.MAXResolution.y;
        }
        
        private IEnumerator CameraZoomIn()
        {
            _cameraView.PixelPerfectCamera.enabled = false;
            _cameraView.Camera.orthographicSize = _cameraView.MAXSize;
            while (_cameraView.Camera.orthographicSize > _cameraView.MINSize)
            {
                float newSize = _cameraView.Camera.orthographicSize;
                newSize -= 0.075f;
                _cameraView.Camera.orthographicSize = newSize;
                yield return 0;
            }
            _cameraView.PixelPerfectCamera.enabled = true;
            _cameraView.PixelPerfectCamera.refResolutionX = (int) _cameraView.MINResolution.x;
            _cameraView.PixelPerfectCamera.refResolutionY = (int) _cameraView.MINResolution.y;
        }

        private void MoveCameraToPlayer()
        {
            var targetPosition = _player.position;
            targetPosition.y += OFFSET_Y;
            var newPosition = targetPosition;
            newPosition.z = OFFSET_Z;
            _cameraView.transform.position = newPosition;
        }

        private void OnStartTriggerEnter(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerView _))
            {
                _followPlayer = false;
                _zoomOutCoroutine = CameraZoomOut().ToObservable().Subscribe();
            }
        }

        private void OnEndTriggerEnter(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerView _))
            {
                _followPlayer = true;
                _zoomInCoroutine = CameraZoomIn().ToObservable().Subscribe();
            }
        }

        public void Cleanup()
        {
            _startTrigger.OnTriggerEnter -= OnStartTriggerEnter;
            _endTrigger.OnTriggerEnter -= OnEndTriggerEnter;
            _zoomInCoroutine?.Dispose();
            _zoomOutCoroutine?.Dispose();
        }
    }
}