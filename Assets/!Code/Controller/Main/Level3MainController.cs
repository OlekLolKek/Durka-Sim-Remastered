using System;
using System.Collections.Generic;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class Level3MainController : MonoBehaviour
    {
        [SerializeField] private string _playerConfigPath = "Animation/PlayerAnimationConfig";
        [SerializeField] private string _coinConfigPath = "Animation/CoinAnimationConfig";
        [SerializeField] private string _bulletConfigPath = "BulletConfig";
        [SerializeField] private string _lemonLaserConfigPath = "LemonLaserConfig";
        [SerializeField] private string _burstLaserConfigPath = "BurstLaserConfig";
        [SerializeField] private string _johnLemonAIConfigPath = "JohnLemonAIConfig";
        [Space]
        [SerializeField] private int _playerHealth = 100;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Transform _bulletSource;
        [SerializeField] private Transform _leftLemonBulletSource;
        [SerializeField] private Transform _rightLemonBulletSource;
        [SerializeField] private List<BulletView> _bullets;
        [SerializeField] private List<BulletEffectView> _bulletEffects;
        [SerializeField] private List<LemonLaserView> _lemonLaserViews;
        [SerializeField] private List<BulletEffectView> _lemonLaserEffects;
        [SerializeField] private List<LemonLaserView> _burstLaserViews;
        [SerializeField] private List<BulletEffectView> _burstLaserEffects;
        [SerializeField] private List<LevelObjectView> _winZones;
        [SerializeField] private List<SyringeView> _coins;

        private readonly Controllers _controllers = new Controllers();

        private void Awake()
        {
            var playerConfig = Resources.Load<SpriteAnimatorConfig>(_playerConfigPath);
            var coinConfig = Resources.Load<SpriteAnimatorConfig>(_coinConfigPath);

            var bulletConfig = Resources.Load<BulletConfig>(_bulletConfigPath);
            var lemonLaserConfig = Resources.Load<BulletConfig>(_lemonLaserConfigPath);
            var burstLaserConfig = Resources.Load<BulletConfig>(_burstLaserConfigPath);

            var johnLemonAIConfig = Resources.Load<AIConfig>(_johnLemonAIConfigPath);

            var inputModel = new InputModel();
            var ammoModel = new AmmoModel();
            var playerLifeModel = new PlayerLifeModel(_playerHealth);
            var playerDataModel = new PlayerDataModel();
            var doorUseModel = new DoorUseModel();

            _controllers.AddController(
                new InputController(inputModel));
            
            _controllers.AddController(
                new PlayerController(_playerView, playerConfig, inputModel,
                    playerLifeModel, playerDataModel, _bullets, _bulletEffects,
                    _bulletSource, ammoModel, bulletConfig, _camera, doorUseModel));
            
            _controllers.AddController(
                new CameraController(_camera.transform, _playerView.transform));
            
            _controllers.AddController(
                new CoinsController(_playerView, _coins, coinConfig,
                    ammoModel));
            
            _controllers.AddController(
                new JohnLemonAI(_lemonLaserViews, _burstLaserViews, 
                    _lemonLaserEffects, _burstLaserEffects,
                    _leftLemonBulletSource, _rightLemonBulletSource, 
                    _playerView.transform, lemonLaserConfig,
                    burstLaserConfig, johnLemonAIConfig));
            
            _controllers.AddController(
                new UIController(playerLifeModel, ammoModel, doorUseModel));
            
            _controllers.AddController(
                new MusicController(_musicAudioSource, playerLifeModel));
            
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

        private void OnDestroy()
        {
            _controllers.Cleanup();
        }
    }
}