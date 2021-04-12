using System;
using System.Collections;
using DG.Tweening;
using DurkaSimRemastered.Interface;
using Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace DurkaSimRemastered
{
    public class FaderView : MonoBehaviour, IUIElement, ICleanup
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _blackColor;
        [SerializeField] private Color _transparentColor;

        private PlayerLifeModel _playerLifeModel;
        private DoorUseModel _doorUseModel;
        private IDisposable _fadeCoroutine;
        private IDisposable _deathCoroutine;
        private IDisposable _winCoroutine;

        public void Initialize(DoorUseModel doorUseModel, PlayerLifeModel playerLifeModel)
        {
            _doorUseModel = doorUseModel;
            _doorUseModel.OnDoorActivated += StartFade;
            _image.color = _blackColor;

            _playerLifeModel = playerLifeModel;
            _playerLifeModel.OnPlayerDied += OnPlayerDied;
            _playerLifeModel.OnPlayerWon += OnPlayerWon;
            
            Hide();
        }

        private void StartFade(DoorView _)
        {
            _fadeCoroutine = Fade().ToObservable().Subscribe();
        }

        private IEnumerator Fade()
        {
            Show();
            yield return new WaitForSeconds(TeleportTimings.PAUSE_DURATION);
            Hide();
        }

        private void OnPlayerDied()
        {
            _deathCoroutine = DeathFade().ToObservable().Subscribe();
        }

        private IEnumerator DeathFade()
        {
            Show();
            yield return new WaitForSeconds(DeathTimings.PAUSE_TIME);
            Hide();
        }

        private void OnPlayerWon()
        {
            _winCoroutine = WinFade().ToObservable().Subscribe();
        }

        private IEnumerator WinFade()
        {
            Show();
            yield return new WaitForSeconds(DeathTimings.PAUSE_TIME);
        }

        public void Show()
        {
            _image.DOColor(_blackColor, TeleportTimings.FADE_IN_DURATION);
        }

        public void ShowInstant()
        {
            _image.color = _blackColor;
        }

        public void Hide()
        {
            _image.DOColor(_transparentColor, TeleportTimings.FADE_OUT_DURATION);
        }

        public void HideInstant()
        {
            _image.color = _transparentColor;
        }

        public void Cleanup()
        {
            _doorUseModel.OnDoorActivated -= StartFade;
            _playerLifeModel.OnPlayerDied -= OnPlayerDied;
            _playerLifeModel.OnPlayerWon -= OnPlayerWon;
            _fadeCoroutine?.Dispose();
            _deathCoroutine?.Dispose();
            _winCoroutine?.Dispose();
        }
    }
}