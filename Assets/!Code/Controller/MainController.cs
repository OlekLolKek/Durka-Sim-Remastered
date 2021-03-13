using System.Collections.Generic;
using Model;
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
        [SerializeField] private Transform _background;
        [SerializeField] private List<BulletView> _bullets;
        [SerializeField] private List<LevelObjectView> _winZones;
        [SerializeField] private List<LevelObjectView> _deathZones;
        [SerializeField] private List<LevelObjectView> _coins;
        
        private Controllers _controllers;

        private void Awake()
        {
            _controllers = new Controllers();
            
            var playerConfig = Resources.Load<SpriteAnimatorConfig>(_playerConfigPath);
            var coinConfig = Resources.Load<SpriteAnimatorConfig>(_coinConfigPath);
            var inputModel = new InputModel();
            
            _controllers.AddController(
                new InputController(inputModel));
            
            _controllers.AddController(
                new PlayerMovement(_playerView, playerConfig, inputModel));
            
            _controllers.AddController(
                new CameraController(_camera.transform, _playerView.transform));
            
            _controllers.AddController(
                new BarrelRotation(_barrel, _camera, _playerView.transform));
            
            _controllers.AddController(
                new BulletsEmitter(_bullets, _muzzle));
            
            _controllers.AddController(
                new CoinsController(_playerView, _coins, coinConfig));
            
            _controllers.AddController(
                new ParallaxController(_camera.transform, _background));
            
            var levelCompleteController = new LevelCompleteController(_playerView, _deathZones, _winZones);
            
            _controllers.Initialize();
        }
        
        private void Update()
        {
            var deltaTime = Time.deltaTime;
            _controllers.Execute(deltaTime);
        }

        private void FixedUpdate()
        {
            _controllers.FixedExecute();
        }

        private void LateUpdate()
        {
            _controllers.LateExecute();
        }
    }
}
