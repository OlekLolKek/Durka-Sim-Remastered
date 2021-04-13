using System;
using System.Collections;
using DurkaSimRemastered.Interface;
using Model;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DurkaSimRemastered
{
    public class Level3CompleteController : ICleanup
    {
        private readonly PlayerLifeModel _playerLifeModel;
        private readonly JohnLemonLifeModel _johnLemonLifeModel;
        private readonly FloorDoorView _floorDoorView;

        private IDisposable _lemonDeathCoroutine;
        private IDisposable _winCoroutine;
        private IDisposable _loseCoroutine;

        public Level3CompleteController(PlayerLifeModel playerLifeModel,
            JohnLemonLifeModel johnLemonLifeModel, FloorDoorView floorDoorView,
            LevelObjectView winZone)
        {
            _playerLifeModel = playerLifeModel;
            _johnLemonLifeModel = johnLemonLifeModel;
            _floorDoorView = floorDoorView;

            _johnLemonLifeModel.OnLemonDied += OnLemonDied;
            _playerLifeModel.OnPlayerDied += OnPlayerLost;
            _playerLifeModel.OnPlayerWon += OnPlayerWon;

            winZone.OnTriggerEnter += OnWinZoneTriggerEnter;
        }

        private void OnLemonDied()
        {
            _lemonDeathCoroutine = OpenDoors().ToObservable().Subscribe();
        }

        private IEnumerator OpenDoors()
        {
            yield return new WaitForSeconds(JohnLemonTimings.DEATH_DELAY);
            _floorDoorView.Open();
        }

        private void OnPlayerWon()
        {
            _winCoroutine = LoadCredits().ToObservable().Subscribe();
        }

        private IEnumerator LoadCredits()
        {
            yield return new WaitForSeconds(DeathTimings.FADE_IN_TIME + DeathTimings.PAUSE_TIME);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void OnPlayerLost()
        {
            _loseCoroutine = Lose().ToObservable().Subscribe();
        }

        private IEnumerator Lose()
        {
            yield return new WaitForSeconds(DeathTimings.FADE_IN_TIME + DeathTimings.PAUSE_TIME);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        private void OnWinZoneTriggerEnter(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerView _))
            {
                _playerLifeModel.Win();
            }
        }

        public void Cleanup()
        {
            _lemonDeathCoroutine?.Dispose();
            _loseCoroutine?.Dispose();
            _winCoroutine?.Dispose();
            _johnLemonLifeModel.OnLemonDied -= OnLemonDied;
            _playerLifeModel.OnPlayerDied -= OnPlayerLost;
            _playerLifeModel.OnPlayerWon -= OnPlayerWon;
        }
    }
}