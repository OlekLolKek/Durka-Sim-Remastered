using System;
using System.Collections;
using System.Collections.Generic;
using DurkaSimRemastered.Interface;
using Model;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DurkaSimRemastered
{
    public sealed class LevelCompleteController : ICleanup
    {
        private IDisposable _playerRespawnCoroutine;
        private IDisposable _changeSceneCoroutine;
        private readonly Vector3 _startPosition;
        private readonly LevelObjectView _characterView;
        private readonly List<LevelObjectView> _deathZones;
        private readonly List<LevelObjectView> _winZones;
        private readonly PlayerLifeModel _playerLifeModel;

        private const int DEATH_PIT_DAMAGE = 9999;

        public LevelCompleteController(LevelObjectView characterView, List<LevelObjectView> deathZones, 
            List<LevelObjectView> winZones, PlayerLifeModel playerLifeModel)
        {
            _startPosition = characterView.transform.position;
            
            _characterView = characterView;
            _deathZones = deathZones;
            _winZones = winZones;
            _playerLifeModel = playerLifeModel;
            _playerLifeModel.OnPlayerDied += OnPlayerDied;
            
            foreach (var deathZone in _deathZones)
            {
                deathZone.OnTriggerEnter += OnDeathZoneTriggerEnter;
            }

            foreach (var winZone in _winZones)
            {
                winZone.OnTriggerEnter += OnWinZoneTriggerEnter;
            }
        }

        private void OnDeathZoneTriggerEnter(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(DEATH_PIT_DAMAGE);
            }
        }

        private void OnWinZoneTriggerEnter(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerView _))
            {
                _playerLifeModel.Win();
                
                if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {
                    _changeSceneCoroutine = ChangeScene(SceneManager.GetActiveScene().buildIndex + 1).ToObservable()
                        .Subscribe();
                }
                else
                {
                    _changeSceneCoroutine = ChangeScene(0).ToObservable()
                        .Subscribe();
                }
            }
        }

        private void OnPlayerDied()
        {
            _playerRespawnCoroutine = Respawn().ToObservable().Subscribe();
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(DeathTimings.FADE_IN_TIME);
            _characterView.transform.position = _startPosition;
            _playerLifeModel.SetHealth(_playerLifeModel.MaxHealth);
            _playerLifeModel.IsDead = false;
        }

        private IEnumerator ChangeScene(int sceneNumber)
        {
            yield return new WaitForSeconds(DeathTimings.FADE_IN_TIME + DeathTimings.PAUSE_TIME);
            SceneManager.LoadScene(sceneNumber);
        }
        
        public void Cleanup()
        {
            foreach (var deathZone in _deathZones)
            {
                deathZone.OnTriggerEnter -= OnDeathZoneTriggerEnter;
            }

            _playerLifeModel.OnPlayerDied -= OnPlayerDied;
            
            _playerRespawnCoroutine?.Dispose();
            _changeSceneCoroutine?.Dispose();
        }
    }
}