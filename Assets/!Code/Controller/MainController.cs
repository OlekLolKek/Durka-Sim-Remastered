using System;
using System.Collections.Generic;
using Model;
using Quests;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private string _playerConfigPath = "Animation/PlayerAnimationConfig";
        [SerializeField] private string _coinConfigPath = "Animation/CoinAnimationConfig";
        [SerializeField] private string _robotConfigPath = "Animation/RobotAnimationConfig";
        [SerializeField] private string _leverConfigPath = "Animation/LeverAnimationConfig";
        [SerializeField] private string _interactionButtonHintConfigPath = "Animation/InteractionButtonAnimationConfig";
        [SerializeField] private string _bulletConfigPath = "Bullet";
        [SerializeField] private string _aiConfigPath = "AIConfig";
        [Space]
        [SerializeField] private int _playerHealth = 100;
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private LevelObjectView _interactionButtonHintView;
        [SerializeField] private Transform _bulletSource;
        [SerializeField] private Transform _background;
        [SerializeField] private List<BulletView> _bullets;
        [SerializeField] private List<BulletParticleSystemView> _bulletParticles;
        [SerializeField] private List<LevelObjectView> _winZones;
        [SerializeField] private List<LevelObjectView> _deathZones;
        [SerializeField] private List<LevelObjectView> _coins;
        [SerializeField] private List<ElevatorView> _elevatorViews;

        private Controllers _controllers;

        private void Awake()
        {
            _controllers = new Controllers();
            
            var interactionButtonHintConfig = Resources.Load<SpriteAnimatorConfig>(_interactionButtonHintConfigPath);
            var playerConfig = Resources.Load<SpriteAnimatorConfig>(_playerConfigPath);
            var coinConfig = Resources.Load<SpriteAnimatorConfig>(_coinConfigPath);
            var robotConfig = Resources.Load<SpriteAnimatorConfig>(_robotConfigPath);
            var leverConfig = Resources.Load<SpriteAnimatorConfig>(_leverConfigPath);

            var aiConfig = Resources.Load<AIConfig>(_aiConfigPath);
            var bulletConfig = Resources.Load<BulletConfig>(_bulletConfigPath);
            
            var inputModel = new InputModel();
            var ammoModel = new AmmoModel();
            var playerLifeModel = new PlayerLifeModel(_playerHealth);
            var playerDataModel = new PlayerDataModel();
            var doorUseModel = new DoorUseModel();
            
            _controllers.AddController(
                new InputController(inputModel));
            
            _controllers.AddController(
                new PlayerController(_playerView, playerConfig, inputModel,
                    playerLifeModel, playerDataModel, _bullets, _bulletParticles, 
                    _bulletSource, ammoModel, bulletConfig, _camera, doorUseModel));
            
            _controllers.AddController(
                new CameraController(_camera.transform, _playerView.transform));
            
            _controllers.AddController(
                new CoinsController(_playerView, _coins, coinConfig,
                    ammoModel));
            
            _controllers.AddController(
                new ParallaxController(_camera.transform, _background));
            
            _controllers.AddController(
                new ElevatorController(_elevatorViews));
            
            _controllers.AddController(
               new EnemiesController(aiConfig, _playerView.transform, robotConfig));
            
            _controllers.AddController(
                new QuestController(leverConfig, playerDataModel,
                    inputModel));
            
            _controllers.AddController(
                new InteractionHintSpriteController(playerDataModel, 
                    interactionButtonHintConfig, _interactionButtonHintView,
                    _playerView));
            
            _controllers.AddController(
                new UIController(playerLifeModel, ammoModel, doorUseModel));
            
            _controllers.AddController(
                new DoorController(playerDataModel, inputModel, doorUseModel));
            
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

        private void OnDestroy()
        {
            _controllers.Cleanup();
        }
    }
}
