using System;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LevelObjectView _playerView;
        [SerializeField] private Transform _barrel;

        private SpriteAnimator _playerAnimator;
        private MainHeroWalker _mainHeroWalker;
        private CameraController _cameraController;
        private AimingMuzzle _aimingMuzzle;
        
        private void Awake()
        {
            SpriteAnimatorConfig playerConfig = Resources.Load<SpriteAnimatorConfig>("PlayerAnimationConfig");
            _playerAnimator = new SpriteAnimator(playerConfig);

            _mainHeroWalker = new MainHeroWalker(_playerView, _playerAnimator);

            _cameraController = new CameraController(_camera.transform, _playerView.transform);

            _aimingMuzzle = new AimingMuzzle(_barrel, _camera);
        }
        
        private void Update()
        {
            _mainHeroWalker.Update();
            _playerAnimator.Update();
            _cameraController.Update();
            _aimingMuzzle.Update();
        }

        private void FixedUpdate()
        {
            
        }

        private void LateUpdate()
        {
            
        }
    }
}
