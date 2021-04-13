using DG.Tweening;
using DurkaSimRemastered.Interface;
using Model;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class Level3MusicController : IInitialize, ICleanup
    {
        private readonly AudioSource _musicAudioSource;
        private readonly PlayerLifeModel _playerLifeModel;
        private readonly JohnLemonLifeModel _johnLemonLifeModel;

        private const float MIN_VOLUME = 0.0f;
        private const float MAX_VOLUME = 0.33f;

        public Level3MusicController(AudioSource musicAudioSource,
            PlayerLifeModel playerLifeModel, JohnLemonLifeModel johnLemonLifeModel)
        {
            _musicAudioSource = musicAudioSource;
            _playerLifeModel = playerLifeModel;
            _johnLemonLifeModel = johnLemonLifeModel;

            _playerLifeModel.OnPlayerDied += OnPlayerDied;
            _johnLemonLifeModel.OnLemonDied += OnJohnLemonDied;
        }

        public void Initialize()
        {
            _musicAudioSource.volume = MIN_VOLUME;
            _musicAudioSource.DOFade(MAX_VOLUME, JohnLemonTimings.MUSIC_START_FADE_TIME);
        }

        private void OnJohnLemonDied()
        {
            _musicAudioSource.volume = 0.0f;
            _johnLemonLifeModel.OnLemonDied -= OnJohnLemonDied;
        }

        private void OnPlayerDied()
        {
            _musicAudioSource.DOFade(MIN_VOLUME, DeathTimings.FADE_OUT_TIME);
            _playerLifeModel.OnPlayerDied -= OnPlayerDied;
        }

        public void Cleanup()
        {
            _playerLifeModel.OnPlayerDied -= OnPlayerDied;
            _johnLemonLifeModel.OnLemonDied -= OnJohnLemonDied;
        }
    }
}