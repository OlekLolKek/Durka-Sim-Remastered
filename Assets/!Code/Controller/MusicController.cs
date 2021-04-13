using DG.Tweening;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class MusicController : IInitialize, ICleanup
    {
        private readonly AudioSource _musicAudioSource;
        private readonly PlayerLifeModel _playerLifeModel;
        
        private const float MIN_VOLUME = 0.0f;
        private const float MAX_VOLUME = 0.33f;

        public MusicController(AudioSource musicAudioSource,
            PlayerLifeModel playerLifeModel)
        {
            _musicAudioSource = musicAudioSource;
            _playerLifeModel = playerLifeModel;
            _playerLifeModel.OnPlayerWon += OnPlayerWon;
        }

        public void Initialize()
        {
            _musicAudioSource.volume = 0.0f;
            _musicAudioSource.DOFade(MAX_VOLUME, DeathTimings.FADE_IN_TIME);
        }

        private void OnPlayerWon()
        {
            _playerLifeModel.OnPlayerWon -= OnPlayerWon;
            _musicAudioSource.DOFade(MIN_VOLUME, DeathTimings.FADE_OUT_TIME);
        }
        
        public void Cleanup()
        {
            _playerLifeModel.OnPlayerWon -= OnPlayerWon;
        }
    }
}