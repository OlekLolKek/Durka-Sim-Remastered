using System.Collections.Generic;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private string _playerConfigPath = "PlayerAnimationConfig";
        [SerializeField] private string _coinConfigPath = "CoinAnimationConfig";
        [SerializeField] private Camera _camera;
        [SerializeField] private LevelObjectView _playerView;
        [SerializeField] private Transform _barrel;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private List<BulletView> _bullets;
        [SerializeField] private List<LevelObjectView> _winZones;
        [SerializeField] private List<LevelObjectView> _deathZones;
        [SerializeField] private List<LevelObjectView> _coins;

        private SpriteAnimator _playerAnimator;
        private SpriteAnimator _coinAnimator;
        private PlayerMovement _playerMovement;
        private CameraController _cameraController;
        private BarrelRotation _barrelRotation;
        private BulletsEmitter _bulletsEmitter;
        private LevelCompleteController _levelCompleteController;
        private CoinsController _coinsController;
        
        private void Awake()
        {
            SpriteAnimatorConfig playerConfig = Resources.Load<SpriteAnimatorConfig>(_playerConfigPath);
            SpriteAnimatorConfig coinConfig = Resources.Load<SpriteAnimatorConfig>(_coinConfigPath);
            
            _playerAnimator = new SpriteAnimator(playerConfig);
            _playerMovement = new PlayerMovement(_playerView, _playerAnimator);
            _cameraController = new CameraController(_camera.transform, _playerView.transform);
            _barrelRotation = new BarrelRotation(_barrel, _camera, _playerView.transform);
            _bulletsEmitter = new BulletsEmitter(_bullets, _muzzle);

            _levelCompleteController = new LevelCompleteController(_playerView, _deathZones, _winZones);

            _coinAnimator = new SpriteAnimator(coinConfig);
            _coinsController = new CoinsController(_playerView, _coins, _coinAnimator);
        }
        
        private void Update()
        {
            _playerAnimator.Update();
            _cameraController.Update();
            _barrelRotation.Update();
            _bulletsEmitter.Update();
            _coinAnimator.Update();
        }

        private void FixedUpdate()
        {
            _playerMovement.FixedUpdate();
        }

        private void LateUpdate()
        {
            
        }
    }
}
