using System;
using Model;
using UnityEngine;
using UnityEngine.UI;


namespace DurkaSimRemastered
{
    public class CreditsMainController : MonoBehaviour
    {
        [SerializeField] private string _playerConfigPath = "Animation/PlayerAnimationConfig";
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private GameObject _creditsCanvas;
        [SerializeField] private Image _fader;
        [SerializeField] private LevelObjectView _johnLemonHead;
        
        private readonly Controllers _controllers = new Controllers();

        private void Start()
        {
            var playerConfig = Resources.Load<SpriteAnimatorConfig>(_playerConfigPath);

            var inputModel = new InputModel();
            var playerDataModel = new PlayerDataModel();
            var creditsMovementModel = new CreditsMovementModel();
            
            _controllers.AddController(
                new InputController(inputModel));
            
            _controllers.AddController(
                new CreditsPlayerController(_playerView, inputModel, 
                    playerConfig, playerDataModel, creditsMovementModel,
                    _johnLemonHead));
            
            _controllers.AddController(
                new CreditsRollController(_creditsCanvas, creditsMovementModel));
            
            _controllers.AddController(
                new CreditsCompleteController(_fader, _musicAudioSource,
                    creditsMovementModel));
            
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