using UnityEngine;
using UnityEngine.Audio;


namespace DurkaSimRemastered
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private UIButtonView _playButton;
        [SerializeField] private UIButtonView _optionsButton;
        [SerializeField] private UIButtonView _exitButton;
        [SerializeField] private UIButtonView _backFromOptionsButton;
        [SerializeField] private UISliderView _masterVolumeSlider;
        [SerializeField] private FaderView _faderView;
        [SerializeField] private RectTransform _mainLayout;
        [SerializeField] private RectTransform _optionsLayout;
        [SerializeField] private AudioMixer _audioMixer;

        [SerializeField] private Transform[] _backgroundTransforms;

        private Controllers _controllers;

        private void Start()
        {
            _controllers = new Controllers();
            
            _controllers.AddController(
                new ButtonsController(_playButton, _optionsButton,
                    _exitButton, _backFromOptionsButton, 
                    _faderView, _mainLayout, _optionsLayout,
                    _masterVolumeSlider, _audioMixer));
            
            _controllers.AddController(
                new BackgroundController(_backgroundTransforms));
            
            _controllers.AddController(
                new FaderController(_faderView));
            
            _controllers.Initialize();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            _controllers.Execute(deltaTime);
        }

        private void OnDestroy()
        {
            _controllers.Cleanup();
        }
    }
}