using System;
using System.Collections;
using DurkaSimRemastered.Interface;
using UniRx;
using UnityEngine;


namespace DurkaSimRemastered
{
    public class ExitButtonController : ICleanup
    {
        private const float EXIT_DELAY_TIME = 0.25f;
        
        private readonly UIButtonView _buttonView;
        private readonly FaderView _faderView;

        private IDisposable _exitCoroutine;

        public ExitButtonController(UIButtonView buttonView, FaderView faderView)
        {
            _buttonView = buttonView;
            _faderView = faderView;
            
            _buttonView.Button.onClick.AddListener(OnExitButtonPressed);
        }

        private void OnExitButtonPressed()
        {
            _exitCoroutine = StartExit().ToObservable().Subscribe();
            _buttonView.AudioSource.Play();
        }

        private IEnumerator StartExit()
        {
            _faderView.Show();
            yield return new WaitForSeconds(EXIT_DELAY_TIME);
            Application.Quit();
        }
        
        public void Cleanup()
        {
            _buttonView.Button.onClick.RemoveAllListeners();
            _exitCoroutine?.Dispose();
        }
    }
}