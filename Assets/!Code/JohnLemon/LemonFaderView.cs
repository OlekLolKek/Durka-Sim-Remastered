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
    public class LemonFaderView : MonoBehaviour, IUIElement, ICleanup
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _blackColor;
        [SerializeField] private Color _transparentColor;

        private JohnLemonLifeModel _johnLemonLifeModel;
        private PlayerLifeModel _playerLifeModel;

        public void Initialize(PlayerLifeModel playerLifeModel)
        {
            ShowInstant();

            _playerLifeModel = playerLifeModel;
            _playerLifeModel.OnPlayerDied += OnPlayerDied;
            _playerLifeModel.OnPlayerWon += OnPlayerWon;

            Hide();
        }

        private void OnPlayerDied()
        {
            Show();
        }

        private void OnPlayerWon()
        {
            Show();
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

        public void Cleanup()
        {
            _playerLifeModel.OnPlayerDied -= OnPlayerDied;
            _playerLifeModel.OnPlayerWon -= OnPlayerWon;
        }
    }
}