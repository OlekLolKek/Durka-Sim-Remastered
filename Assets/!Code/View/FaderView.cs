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

        private DoorUseModel _doorUseModel;
        private IDisposable _fadeCoroutine;

        public void Initialize(DoorUseModel doorUseModel)
        {
            _doorUseModel = doorUseModel;
            _doorUseModel.OnDoorActivated += StartFade;
            _image.color = _blackColor;
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

        public void Show()
        {
            _image.DOColor(_blackColor, TeleportTimings.FADE_IN_DURATION);
        }

        public void Hide()
        {
            _image.DOColor(_transparentColor, TeleportTimings.FADE_OUT_DURATION);
        }

        public void Cleanup()
        {
            _doorUseModel.OnDoorActivated -= StartFade;
            _fadeCoroutine?.Dispose();
        }
    }
}