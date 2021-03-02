using System.Collections.Generic;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LevelObjectView _playerView;
        [SerializeField] private Transform _barrel;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private List<BulletView> _bullets;

        private SpriteAnimator _playerAnimator;
        private PlayerMovement _playerMovement;
        private CameraController _cameraController;
        private BarrelRotation _barrelRotation;
        private BulletsEmitter _bulletsEmitter;
        
        private void Awake()
        {
            SpriteAnimatorConfig playerConfig = Resources.Load<SpriteAnimatorConfig>("PlayerAnimationConfig");
            _playerAnimator = new SpriteAnimator(playerConfig);

            _playerMovement = new PlayerMovement(_playerView, _playerAnimator);

            _cameraController = new CameraController(_camera.transform, _playerView.transform);

            _barrelRotation = new BarrelRotation(_barrel, _camera, _playerView.transform);

            _bulletsEmitter = new BulletsEmitter(_bullets, _muzzle);
        }
        
        private void Update()
        {
            _playerMovement.Update();
            _playerAnimator.Update();
            _cameraController.Update();
            _barrelRotation.Update();
            _bulletsEmitter.Update();
        }

        private void FixedUpdate()
        {
            
        }

        private void LateUpdate()
        {
            
        }
    }
}
